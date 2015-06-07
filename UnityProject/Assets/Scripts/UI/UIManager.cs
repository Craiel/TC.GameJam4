using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts;
using Assets.Scripts.Contracts;
using Assets.Scripts.Logic;

public class UIManager : MonoBehaviour 
{
    [SerializeField]
    private GameplayManager gameplayManager;

    [SerializeField]
    private GameObject playerSelect;

    [SerializeField]
    private GameObject CombatTextPrefab;

    public List<UIPlayerManager> players;
    private IList<ICharacter> characters;

    public void Init(IList<ICharacter> characters)
    {   
        for (var i = 0; i < characters.Count; i++)
        {
            players[i].Init(characters[i], this);
        }
    }

    private void Update()
    {
        if(!gameplayManager.IsPlaying)
        {
            Dictionary<ICharacter, MechLoadouts.MechLoadout> characters = new Dictionary<ICharacter, MechLoadouts.MechLoadout>();
            foreach(UIPlayerManager playerManager in players)
            {
                if(playerManager.CurrentState == UIPlayerManager.UIState.Joined)
                {
                    return;
                }
                if(playerManager.CurrentState == UIPlayerManager.UIState.Ready)
                {
                    characters.Add(playerManager.Character, playerManager.Loadout);
                }
            }

            //TODO: Make this actually 2, dummy
            if(characters.Count > 0)
            {
                foreach(ICharacter character in characters.Keys)
                {
                    character.SetBaseStats(characters[character].BasicStats);
                }

                gameplayManager.SetupMatch(new List<ICharacter>(characters.Keys));
                playerSelect.SetActive(false);
            }
        }
        else
        {
            foreach(CombatResult combatResult in Combat.PollResults())
            {
                GameObject combatText = Instantiate(CombatTextPrefab) as GameObject;
                combatText.transform.SetParent(this.transform);

                if(combatResult.WasMiss)
                {
                    combatText.GetComponent<CombatText>().Init("miss", new Color(0.6f, 0.6f, 0.6f), combatResult.Location);
                }
                else
                {
                    combatText.GetComponent<CombatText>().Init(Mathf.RoundToInt(combatResult.DamageDealtTotal).ToString(), new Color(1, 0.4f, 0.4f), combatResult.Location);
                }
            }
        }
    }
}