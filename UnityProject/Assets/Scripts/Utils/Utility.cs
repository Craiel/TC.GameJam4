using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Logic;
using Assets.Scripts.Logic.Enums;
using Assets.Scripts.Weapons;
using Eppy;

public class Utility {

	private static Utility m_Instance = new Utility();
	public static Utility Instance { get { return m_Instance; } }
	
	private Utility() {}

	private Dictionary<StatType,string> 	StatAbbreviations { get; set; }
	private Dictionary<StatType,string>		StatUnits { get; set; }
	private Dictionary<DamageType,string> 	DamageAbbreviations { get; set; }
	private Dictionary<CombatType,string> 	WeaponAbbreviations { get; set; }

	public void Load() {

		this.StatAbbreviations	 = new Dictionary<StatType, string>();
		this.StatUnits			 = new Dictionary<StatType, string>();
		this.DamageAbbreviations = new Dictionary<DamageType, string>();
		this.WeaponAbbreviations = new Dictionary<CombatType, string>();

		StatAbbreviations.Add (StatType.Velocity, "MOV");
		StatAbbreviations.Add (StatType.RotationSpeed, "ROT");
		StatAbbreviations.Add (StatType.Damage, "DMG");
		StatAbbreviations.Add (StatType.RangedAccuracy, "RNG");
		StatUnits.Add (StatType.RangedAccuracy, "%");
		StatAbbreviations.Add (StatType.MeleeAccuracy, "MEL");
		StatUnits.Add (StatType.MeleeAccuracy, "%");
		StatAbbreviations.Add (StatType.TargetingDistance, "DIS");
		StatUnits.Add (StatType.TargetingDistance, "m");
		StatAbbreviations.Add (StatType.TargetingLockTime, "LCK");
		StatUnits.Add (StatType.TargetingLockTime, "s");
		StatAbbreviations.Add (StatType.Health, "HUL");
		StatAbbreviations.Add (StatType.Armor, "ARM");
		StatAbbreviations.Add (StatType.Shield, "SHD");
		StatAbbreviations.Add (StatType.HeatGeneration, "HET");
		StatAbbreviations.Add (StatType.HeatCoolingRate, "COL");
		StatUnits.Add (StatType.HeatCoolingRate, "/s");

		WeaponAbbreviations.Add (CombatType.Melee, "MEL");
		WeaponAbbreviations.Add (CombatType.Ranged, "RNG");

		DamageAbbreviations.Add (DamageType.Energy, "ENG");
		DamageAbbreviations.Add (DamageType.Projectile, "PHY");
		DamageAbbreviations.Add (DamageType.Heat, "HEA");

	}


	public string ValueWithUnits(StatType statType, double value) {

		if(StatUnits.ContainsKey(statType)) {

			string code = StatUnits[statType];

			if(code == "%") {
				value = value * 100;
				return Convert.ToString(Math.Round (value, 0)) + StatUnits[statType];
			
			} else if (code == "s") {
				return Convert.ToString(Math.Round (value, 2)) + StatUnits[statType];
			}

			return Convert.ToString(Math.Round (value, 0)) + StatUnits[statType];
		}

		if (statType == StatType.Velocity) {
			return Math.Round (value, 2) + StatUnits[statType];
		} else if (statType == StatType.RotationSpeed) {
			return Convert.ToString(Math.Round (value, 2)) + StatUnits[statType];
		}

		return Convert.ToString(Math.Round (value, 0));

	}

	public string GetCode(StatType statType) {

		if(StatAbbreviations.ContainsKey(statType)) {
			return StatAbbreviations[statType];
		}
		return "";

	}

	public Eppy.Tuple<string,string,string> GetCode(BaseWeapon weapon) {

		string s1 = ""; //DMG
		string s2 = ""; //RNG
		string s3 = ""; //PHY

		if (weapon is WeaponRanged) {
			s1 = "DMG";
			s2 = "RNG";
			s3 = DamageAbbreviations [weapon.DamageType];

		} else if (weapon is WeaponMelee) {
			s1 = "DMG";
			s2 = "MEL";
			s3 = DamageAbbreviations [weapon.DamageType];

		} else if (weapon is WeaponColumn) {
			s1 = "DMG";
			s2 = "RNG";
			s3 = DamageAbbreviations [weapon.DamageType];

		} else if (weapon is WeaponGrapple) {
			s1 = "DMG";
			s2 = "GRP";
			s3 = DamageAbbreviations [weapon.DamageType];
			
		} else if (weapon is WeaponHeal) {
			s1 = "HEL";

		} else if (weapon is WeaponBomb) {
			s1 = "DMG";
			s2 = "BMB";
			s3 = DamageAbbreviations [weapon.DamageType];

		} else if (weapon is WeaponHeat) {
			s1 = "DMG";
			s2 = "HEA";
			s3 = DamageAbbreviations [weapon.DamageType];
		
			
		} else if (weapon is WeaponHoming) {
			s1 = "DMG";
			s2 = "HOM";
			s3 = DamageAbbreviations [weapon.DamageType];

		} else if (weapon is WeaponInvisibility) {
			s1 = "TIM";
			s2 = "INV";
			
		} else if (weapon is WeaponSlow) {
			s1 = "SLW";
			
		} else if (weapon is WeaponSpeedBoost) {
			s1 = "SPD";
		}

		return new Tuple<string, string, string> (s1,s2,s3);
	}

}
