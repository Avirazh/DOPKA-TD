using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lossy.ScriptableObjects.Config
{
    [CreateAssetMenu(fileName = "CreditsConfig", menuName = "Configs/CreditsConfigScriptableObject",
        order = 1)]
    public class CreditsConfigScriptableObject : ScriptableObject
    {
        [SerializeField] private List<CreditPair> _creditPairs;

        public IReadOnlyList<CreditPair> CreditPairs => _creditPairs;
    }

    [Serializable]
    public class CreditPair
    {
        public CreditsJobTitle JobTitle;
        public string CreditName;
    }

    public enum CreditsJobTitle
    {
        Community = 1,
        Production = 2,
        GameDesign = 3,
        Programming = 4,
        QualityAssurance = 5,
        ArtDirection = 6,
        CharacterArt = 7,
        TechnicalArt = 8,
        VisualEffects = 9,
        Rigging = 10,
        UserInterfaceOrExperience = 11,
        Animation = 12,
        Marketing = 13, 
        Audio = 14,
        VoiceActing = 15,
        AlacrityArthouse = 16,
        VideoProduction = 17,
        AdditionalArtDevelopment = 18,
        Cinematics = 19,
        MusicCredits = 20,
        Platform = 21,
        DataPlatforms = 22,
        Publishing = 23,
        LiveGameOps = 24,
        CustomerSupport = 25,
        HumanResources = 26,
        Localization = 27,
        CreativeServices = 28,
        MarketingCommunications = 29,
        IT = 30,
        OfficeServices = 31,
        CEO = 32,
    }
}