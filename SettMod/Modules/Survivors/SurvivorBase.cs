﻿using BepInEx.Configuration;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SettMod.Modules.Survivors
{
    internal abstract class SurvivorBase
    {
        internal static SurvivorBase instance;

        internal abstract BodyInfo bodyInfo { get; set; }
        internal abstract string bodyName { get; set; }

        internal abstract GameObject bodyPrefab { get; set; }
        internal abstract ConfigEntry<bool> characterEnabled { get; set; }
        internal abstract Type characterMainState { get; set; }
        internal abstract UnlockableDef characterUnlockableDef { get; set; }
        internal abstract CustomRendererInfo[] customRendererInfos { get; set; }
        internal abstract GameObject displayPrefab { get; set; }

        internal string fullBodyName => bodyName + "Body";
        internal abstract List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules { get; set; }
        internal abstract ItemDisplayRuleSet itemDisplayRuleSet { get; set; }
        internal abstract int mainRendererIndex { get; set; }
        internal abstract float sortPosition { get; set; }

        internal virtual void Initialize()
        {
            instance = this;
            InitializeCharacter();
        }

        internal virtual void InitializeCharacter()
        {
            if (characterEnabled.Value)
            {
                InitializeUnlockables();

                bodyPrefab = Modules.Prefabs.CreatePrefab(bodyName + "Body", "mdl" + bodyName, bodyInfo);
                bodyPrefab.GetComponent<EntityStateMachine>().mainStateType = new EntityStates.SerializableEntityStateType(characterMainState);

                Modules.Prefabs.SetupCharacterModel(bodyPrefab, customRendererInfos, mainRendererIndex);

                displayPrefab = Modules.Prefabs.CreateDisplayPrefab(bodyName + "Display", bodyPrefab, bodyInfo);
                displayPrefab.AddComponent<MenuSound>();

                Modules.Prefabs.RegisterNewSurvivor(bodyPrefab, displayPrefab, new Color(.3781f, .1324f, .4894f), bodyName.ToUpper(), characterUnlockableDef, sortPosition, bodyInfo.bodyNameToken);

                InitializeHitboxes();
                InitializeSkills();
                InitializeSkins();
                InitializeItemDisplays();
                InitializeDoppelganger();
            }
        }

        internal virtual void InitializeDoppelganger()
        {
            Modules.Prefabs.CreateGenericDoppelganger(instance.bodyPrefab, bodyName + "MonsterMaster", "Merc");
        }

        internal virtual void InitializeHitboxes()
        {
        }

        internal virtual void InitializeItemDisplays()
        {
            CharacterModel characterModel = bodyPrefab.GetComponentInChildren<CharacterModel>();

            itemDisplayRuleSet = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();
            itemDisplayRuleSet.name = "idrs" + bodyName;

            characterModel.itemDisplayRuleSet = itemDisplayRuleSet;
        }

        internal virtual void InitializeSkills()
        {
        }

        internal virtual void InitializeSkins()
        {
        }

        internal virtual void InitializeUnlockables()
        {
        }

        internal virtual void SetItemDisplays()
        {
        }
    }
}