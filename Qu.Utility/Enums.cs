
using System.ComponentModel;

namespace Qu.Utility
{
    public class Enums
    {
        #region Lookup

        public enum Lookup : int
        {
            Message_1 = 1,
            MessageType_2 = 2,
            Events_3 = 3,
            EventType_4 = 4,
            EventStatus_5 = 5,
            AuthenticationTypes_6 = 6,
            Platform_7 = 7,
            Language_8 = 8,
            Theme_9 = 9,
            LogCategory_10 = 10,
            Application_11 = 11,
            WorkModel_12 = 12,
            GenderType_13 = 13,
            JobBank_14 = 14,
            EmploymentType_15 = 15,
            EducationLevel_16 = 16,
            ExperienceLevel_17 = 17,
        }

        #endregion

        #region Message

        [Description("1")]
        public enum Message : int
        {
            Custom = 1,
            Number = 2,
            Record = 3,
            Parameter = 4,
            Exception = 5,
            Validation = 6,
            Authorize = 7,
        }

        #endregion

        #region MessageType

        [Description("2")]
        public class MessageType
        {
            [Description("1")]
            public enum Custom : int
            {
                Value = 1, // ThirtPartyId
            }

            [Description("2")]
            public enum Number : int
            {
                MustEqualZero = 1,
                MustNotEqualZero = 2,
                MustBiggerThanZero = 3,
                MustLessThanZero = 4,
            }

            [Description("3")]
            public enum Record : int
            {
                NotFound = 1,
                Updated = 2,
                Created = 3,
                Deleted = 4,
                Listed = 5,
                AlreadyExist = 6,
                Deactivated = 7
            }

            [Description("4")]
            public enum Parameter : int
            {
                TryToDefine = 1,
                DefineSuccess = 2,
                DefineFailed = 3,
            }

            [Description("5")]
            public enum Exception : int
            {
                Unexpected = 1,
                NullReference = 2,
            }

            [Description("6")]
            public enum Validation : int
            {
                CheckingValidation = 1,
                ValidationError = 2,
            }

            [Description("7")]
            public enum Authorize : int
            {
                CheckingAuthorize = 1,
                UnauthorizedAccess = 2,
                AuthorizedSuccess = 3,
                AuthorizationMissing = 4,
                TokenInvalidOrExpired = 5,
                LoggedIn = 6,
                LoggedOut = 7,
                Registered = 8,
                FailedLogin = 9,
                FailedLogout = 10,
                WrongPassword = 11,
                WrongEmail = 12,
                WrongPhone = 13,
                WrongIdNo = 14,
                WrongUsername = 15,
                WrongUsernameOrPassword = 16,
                TokenIsMissing = 17,
            }
        }

        #endregion

        #region Events

        [Description("3")]
        public enum Events : int
        {
            Http = 1,
            Database = 2,
            Authentication = 3,
            Authorization = 4,
        }

        #endregion

        #region EventType

        [Description("4")]
        public class EventType
        {
            [Description("1")]
            public enum Http : int
            {
                Request = 1, // ThirtPartyId
                Response = 2,
            }

            [Description("2")]
            public enum Database : int
            {
                Create = 1,
                Read = 2,
                Update = 3,
                Delete = 4,
                List = 5
            }

            [Description("3")]
            public enum Authentication : int
            {
                LoggedIn = 1,
                LoggedOut = 2,
                Registered = 3,
                FailedLogin = 4,
                FailedLogout = 5,
                WrongPassword = 6,
                WrongEmail = 7,
                WrongPhone = 8,
                WrongIdNo = 9,
                WrongUsername = 10,
            }

            [Description("4")]
            public enum Authorization : int
            {
                NotAuthorized = 1,
                AuthorizedCompany = 2,
                AuthorizedUser = 3,
            }
        }

        #endregion

        #region EventStatus

        [Description("5")]
        public enum EventStatus : short
        {
            Success = 1,
            Info = 2,
            Warning = 3,
            Danger = 4,
        }

        #endregion

        #region AuthenticationTypes

        [Description("6")]
        public enum AuthenticationTypes : int
        {
            ByCompany = 0,
            ByUser = 1,
        }

        #endregion

        #region Platform

        [Description("7")]
        public enum Platform : short
        {
            Development = 1,
            Testing = 2,
            Staging = 3,
            Production = 4
        };

        #endregion

        #region Language

        [Description("8")]
        public enum Language : short
        {
            Turkish = 1,
            English = 2
        };

        #endregion

        #region Theme

        [Description("9")]
        public enum Theme : short
        {
            Default = 1
        };

        #endregion

        #region LogCategory

        [Description("10")]
        public enum LogCategory
        {
            Trace_0 = 0,
            Debug_1 = 1,
            Information_2 = 2,
            Warning_3 = 3,
            Error_4 = 4,
            Critical_5 = 5,
            None_6 = 6
        }

        #endregion

        #region Application

        [Description("11")]
        public enum Application : int
        {
            IG_Api_1 = 1,
            IG_Service_2 = 2,
            IG_Web_3 = 3,
            IG_Log_4 = 4,
            IG_SQL_5 = 5,
        }

        #endregion

        #region WorkModel

        [Description("12")]
        public enum WorkModel : int
        {
            Remote_1 = 1,
            Hybrid_2 = 2,
            OnSite_3 = 3,
        }

        #endregion

        #region GenderType

        [Description("13")]
        public enum GenderType : int
        {
            NotSelected_1 = 1,
            Male_2 = 2,
            Female_3 = 3,
        }

        #endregion

        #region JobBank

        [Description("14")]
        public enum JobBank : int
        {
            Fiba_1 = 1,
            Ziraat_2 = 2,
            Garanti_3 = 3,
            Finansbank_4 = 4,
            Enpara_5 = 5,
        }

        #endregion

        #region EmploymentType

        [Description("15")]
        public enum EmploymentType : int
        {
            FullTime_1 = 1,  // Tam zamanlı çalışma
            PartTime_2 = 2,  // Yarı zamanlı çalışma
            Contract_3 = 3,  // Sözleşmeli çalışma
            Seasonal_4 = 4, // Sezonluk / Dönemlik
            Internship_5 = 5 // Stajyerlik
        }

        #endregion

        #region EducationLevel

        [Description("16")]
        public enum EducationLevel : int
        {
            HighSchool_1 = 1,      // Lise
            Associate_2 = 2,       // Ön lisans
            Bachelor_3 = 3,        // Lisans
            Master_4 = 4,          // Yüksek lisans mezunu: Lisansüstü eğitim programını tamamlayarak yüksek lisans diploması alınır.
            Doctorate_5 = 5,       // Doktora mezunu: Doktora programını tamamlayarak doktora derecesi alınır.
            PostDoctorate_6 = 6,   // Post-doktora: Doktora sonrası araştırma yaparak akademik kariyerine devam edenler.
            Professional_7 = 7     // Profesyonel derece: Hukuk, tıp gibi alanlarda uzmanlık gerektiren profesyonel dereceler.
        }

        #endregion

        #region ExperienceLevel

        [Description("17")]
        public enum ExperienceLevel : int
        {
            Entry_1 = 1,
            Mid_2 = 2,
            Senior_3 = 3,
            Expert_4 = 4,
        }

        #endregion
    }
}