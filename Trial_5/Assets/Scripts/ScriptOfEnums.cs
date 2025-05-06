using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EvaluateCardsEnum
{
    None,
    ShowInfoCard,
    MatchCards,
}

public enum InfoCategoryEnum
{
    None,
    Trigger,
    Symptom,
    Treatment,
}

public enum InformationTypeEnum
{
    General,
    Definition,
}

public enum UserTypeEnum
{
    Doctor,
    Patient,
    Random_User,
}

public enum IncreasOrDecreaseEnum
{
    None,
    Increase,
    Decrease,
}

public enum WordTypeEnum
{
    Noun,
    Verb,
}

public enum VariableTypeForAPEnum
{
    None,
    Integer,
    Decimal,
    Text,
    Enum,
    Date,
    Contact,
}

public enum InhalerColorEnum
{
    Blue,
    Brown,
    Orange,
    Purple,
}

public enum MonthDisplayEnum
{
    Number,
    Name,
    Abbreviation,
}

public enum GenderEnum
{
    Male,
    Female,
    Unknown,
}

public enum SettingsTypeEnum
{
    Boolean,
    Decimal,
    Enum,
}

public enum LanguageEnum
{
    English,
    Arabic,
}

public enum DraggableTypeEnum
{
    Letter = 1,
    Inhaler_Object = 2,
    Other = 0,
}

public enum APEnumTypeEnum
{
    Gender,
    Inhaler_Color,
}

public enum ProcessStatusEnum
{
    Idle,
    InProgress,
    Complete,
    Cancelled,
}

public enum TalkingStatusEnum
{
    Not_Talking,
    Starting,
    Talking,
    Cancelled,
    Completed,
}

public enum TutorialSubjectTypeEnum
{
    UI,
    World_Object,
    Other,
    None,
}