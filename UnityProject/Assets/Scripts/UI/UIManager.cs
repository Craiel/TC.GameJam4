using UnityEngine;
using System.Collections.Generic;

using Assets.Scripts;
using Assets.Scripts.Contracts;
using Assets.Scripts.Logic;

using JetBrains.Annotations;

public class UIManager : MonoBehaviour
{
    private bool waitingForCharacterSelection = true;

    [SerializeField]
    private GameplayManager gameplayManager;

    [SerializeField]
    private GameObject playerSelect;

    [SerializeField]
    private GameObject gameScene;

    [SerializeField]
    private GameObject CombatTextPrefab;

    [SerializeField]
    private List<PlayerGamePanel> playerGamePanels;

    public List<UIPlayerManager> players;
    private IList<ICharacter> characters;

    public void Init(IList<ICharacter> characters)
    {   
        for (var i = 0; i < characters.Count; i++)
        {
            players[i].Init(characters[i]);
        }

        playerSelect.SetActive(true);
        gameScene.SetActive(false);
    }
    
    private IDictionary<ICharacter, MechLoadouts.MechLoadout> GetReadyPlayers()
    {
        var result = new Dictionary<ICharacter, MechLoadouts.MechLoadout>();
        foreach (UIPlayerManager playerManager in this.players)
        {
            if (playerManager.CurrentState == UIPlayerManager.UIState.Ready)
            {
                result.Add(playerManager.Character, playerManager.Loadout);
            }
        }

        return result;
    }

    private void SetupMatch(IDictionary<ICharacter, MechLoadouts.MechLoadout> players)
    {
        this.playerSelect.SetActive(false);
        this.gameScene.SetActive(true);

        int playerPanelIndex = 0;
        foreach (ICharacter character in players.Keys)
        {
            character.SetBaseStats(players[character].BasicStats);

            if (character.InputDevice != null)
            {
                this.playerGamePanels[playerPanelIndex].Init(character);
            }
            else
            {
                this.playerGamePanels[playerPanelIndex].gameObject.SetActive(false);
            }

            playerPanelIndex++;
        }

        this.gameplayManager.SetupMatch(new List<ICharacter>(players.Keys));
    }

    private void SetupPlayerSelection()
    {
        this.playerSelect.SetActive(true);
        this.gameScene.SetActive(false);

        foreach (PlayerGamePanel gamePanel in this.playerGamePanels)
        {
            gamePanel.gameObject.SetActive(false);
        }

        this.waitingForCharacterSelection = true;
    }

    [UsedImplicitly]
    private void Update()
    {
        if(!this.gameplayManager.IsPlaying)
        {
            if (this.gameplayManager.HasEnded && !this.waitingForCharacterSelection)
            {
                this.SetupPlayerSelection();
            }
            else if (this.gameplayManager.HasEnded && this.waitingForCharacterSelection)
            {
                IDictionary<ICharacter, MechLoadouts.MechLoadout> readyPlayers = this.GetReadyPlayers();
                if (readyPlayers.Count > 1)
                {
                    this.SetupMatch(readyPlayers);
                }
            }
        }
        else
        {
            foreach(CombatResult combatResult in Combat.PollResults())
            {
                GameObject combatText = Instantiate(this.CombatTextPrefab);
                combatText.transform.SetParent(this.transform);

                if(combatResult.WasMiss)
                {
                    combatText.GetComponent<CombatText>().Init("miss", new Color(0.6f, 0.6f, 0.6f), combatResult.Location);
                }
                else
                {
                    combatText.GetComponent<CombatText>().Init(Mathf.RoundToInt(combatResult.DamageDealtTotal).ToString(), new Color(1, 0.1f, 0.2f), combatResult.Location);
                }
            }
        }
    }
}