namespace RpmReaderNet
{
    internal static class RpmConstants
    {
        public const int HEADER_SIGBASE = 256;

        public enum rpmTagType
        {
            RPM_NULL_TYPE = 0,
            RPM_CHAR_TYPE = 1,
            RPM_INT8_TYPE = 2,
            RPM_INT16_TYPE = 3,
            RPM_INT32_TYPE = 4,
            /*    RPM_INT64_TYPE    = 5,   ---- These aren't supported (yet) */
            RPM_STRING_TYPE = 6,
            RPM_BIN_TYPE = 7,
            RPM_STRING_ARRAY_TYPE = 8,
            RPM_I18NSTRING_TYPE = 9
        }

        public enum rpmTag
        {
            RPMTAG_NOT_FOUND = -1,

            /* Retrofit (and uniqify) signature tags for use by rpmTagGetName() and rpmQuery. */
            /* the md5 sum was broken *twice* on big endian machines */
            /* XXX 2nd underscore prevents tagTable generation */
            RPMTAG_SIG_BASE = HEADER_SIGBASE,
            RPMTAG_SIGSIZE = RPMTAG_SIG_BASE + 1,    /* i */
            RPMTAG_SIGLEMD5_1 = RPMTAG_SIG_BASE + 2,    /* internal - obsolete */
            RPMTAG_SIGPGP = RPMTAG_SIG_BASE + 3,    /* x */
            RPMTAG_SIGLEMD5_2 = RPMTAG_SIG_BASE + 4,    /* x internal - obsolete */
            RPMTAG_SIGMD5 = RPMTAG_SIG_BASE + 5,    /* x */
            RPMTAG_SIGGPG = RPMTAG_SIG_BASE + 6,    /* x */
            RPMTAG_SIGPGP5 = RPMTAG_SIG_BASE + 7,    /* internal - obsolete */

            RPMTAG_BADSHA1_1 = RPMTAG_SIG_BASE + 8,    /* internal - obsolete */
            RPMTAG_BADSHA1_2 = RPMTAG_SIG_BASE + 9,    /* internal - obsolete */
            RPMTAG_PUBKEYS = RPMTAG_SIG_BASE + 10,   /* s[] */
            RPMTAG_DSAHEADER = RPMTAG_SIG_BASE + 11,   /* x */
            RPMTAG_RSAHEADER = RPMTAG_SIG_BASE + 12,   /* x */
            RPMTAG_SHA1HEADER = RPMTAG_SIG_BASE + 13,   /* s */
            RPMTAG_LONGSIGSIZE = RPMTAG_SIG_BASE + 14,   /* l */
            RPMTAG_LONGARCHIVESIZE = RPMTAG_SIG_BASE + 15,   /* l */

            RPMTAG_NAME = 1000, /* s */

            //#define RPMTAG_N        RPMTAG_NAME     /* s */
            RPMTAG_VERSION = 1001, /* s */

            //#define RPMTAG_V        RPMTAG_VERSION  /* s */
            RPMTAG_RELEASE = 1002, /* s */

            //#define RPMTAG_R        RPMTAG_RELEASE  /* s */
            RPMTAG_EPOCH = 1003, /* i */

            //#define RPMTAG_E        RPMTAG_EPOCH    /* i */
            //#define RPMTAG_SERIAL   RPMTAG_EPOCH    /* i backward comaptibility */
            RPMTAG_SUMMARY = 1004, /* s{} */

            RPMTAG_DESCRIPTION = 1005, /* s{} */
            RPMTAG_BUILDTIME = 1006, /* i */
            RPMTAG_BUILDHOST = 1007, /* s */
            RPMTAG_INSTALLTIME = 1008, /* i */
            RPMTAG_SIZE = 1009, /* i */
            RPMTAG_DISTRIBUTION = 1010, /* s */
            RPMTAG_VENDOR = 1011, /* s */
            RPMTAG_GIF = 1012, /* x */
            RPMTAG_XPM = 1013, /* x */
            RPMTAG_LICENSE = 1014, /* s */

            //#define RPMTAG_COPYRIGHT RPMTAG_LICENSE /* s backward comaptibility */
            RPMTAG_PACKAGER = 1015, /* s */

            RPMTAG_GROUP = 1016, /* s{} */
                                 /*@-enummemuse@*/
            RPMTAG_CHANGELOG = 1017,
            /*@=enummemuse@*/
            RPMTAG_SOURCE = 1018, /* s[] */
            RPMTAG_PATCH = 1019, /* s[] */
            RPMTAG_URL = 1020, /* s */
            RPMTAG_OS = 1021, /* s legacy used int */
            RPMTAG_ARCH = 1022, /* s legacy used int */
            RPMTAG_PREIN = 1023, /* s */
            RPMTAG_POSTIN = 1024, /* s */
            RPMTAG_PREUN = 1025, /* s */
            RPMTAG_POSTUN = 1026, /* s */
            RPMTAG_OLDFILENAMES = 1027, /* s[] obsolete */
            RPMTAG_FILESIZES = 1028, /* i */
            RPMTAG_FILESTATES = 1029, /* c */
            RPMTAG_FILEMODES = 1030, /* h */
            RPMTAG_FILEUIDS = 1031,
            RPMTAG_FILEGIDS = 1032,
            RPMTAG_FILERDEVS = 1033, /* h */
            RPMTAG_FILEMTIMES = 1034, /* i */
            RPMTAG_FILEMD5S = 1035, /* s[] */
            RPMTAG_FILELINKTOS = 1036, /* s[] */
            RPMTAG_FILEFLAGS = 1037, /* i */
                                     /*@-enummemuse@*/
            RPMTAG_ROOT = 1038,
            /*@=enummemuse@*/
            RPMTAG_FILEUSERNAME = 1039, /* s[] */
            RPMTAG_FILEGROUPNAME = 1040, /* s[] */
                                         /*@-enummemuse@*/
            RPMTAG_EXCLUDE = 1041,
            RPMTAG_EXCLUSIVE = 1042,
            /*@=enummemuse@*/
            RPMTAG_ICON = 1043,
            RPMTAG_SOURCERPM = 1044, /* s */
            RPMTAG_FILEVERIFYFLAGS = 1045, /* i */
            RPMTAG_ARCHIVESIZE = 1046, /* i */
            RPMTAG_PROVIDENAME = 1047, /* s[] */

            //#define RPMTAG_PROVIDES RPMTAG_PROVIDENAME      /* s[] */
            //#define RPMTAG_P        RPMTAG_PROVIDENAME      /* s[] */
            RPMTAG_REQUIREFLAGS = 1048, /* i */

            RPMTAG_REQUIRENAME = 1049, /* s[] */

            //#define RPMTAG_REQUIRES RPMTAG_REQUIRENAME      /* s[] */
            //#define RPMTAG_D        RPMTAG_REQUIRENAME      /* s[] */
            RPMTAG_REQUIREVERSION = 1050, /* s[] */

            RPMTAG_NOSOURCE = 1051,
            RPMTAG_NOPATCH = 1052,
            RPMTAG_CONFLICTFLAGS = 1053, /* i */
            RPMTAG_CONFLICTNAME = 1054, /* s[] */

            //#define RPMTAG_CONFLICTS RPMTAG_CONFLICTNAME    /* s[] */
            //#define RPMTAG_C        RPMTAG_CONFLICTNAME     /* s[] */
            RPMTAG_CONFLICTVERSION = 1055, /* s[] */

            RPMTAG_DEFAULTPREFIX = 1056,
            RPMTAG_BUILDROOT = 1057,
            RPMTAG_INSTALLPREFIX = 1058,
            RPMTAG_EXCLUDEARCH = 1059,
            RPMTAG_EXCLUDEOS = 1060,
            RPMTAG_EXCLUSIVEARCH = 1061,
            RPMTAG_EXCLUSIVEOS = 1062,
            RPMTAG_AUTOREQPROV = 1063,
            RPMTAG_RPMVERSION = 1064, /* s */
            RPMTAG_TRIGGERSCRIPTS = 1065, /* s[] */
            RPMTAG_TRIGGERNAME = 1066, /* s[] */
            RPMTAG_TRIGGERVERSION = 1067, /* s[] */
            RPMTAG_TRIGGERFLAGS = 1068, /* i */
            RPMTAG_TRIGGERINDEX = 1069, /* i */
            RPMTAG_VERIFYSCRIPT = 1079, /* s */
            RPMTAG_CHANGELOGTIME = 1080, /* i */
            RPMTAG_CHANGELOGNAME = 1081, /* s[] */
            RPMTAG_CHANGELOGTEXT = 1082, /* s[] */
                                         /*@-enummemuse@*/
            RPMTAG_BROKENMD5 = 1083,
            /*@=enummemuse@*/
            RPMTAG_PREREQ = 1084,
            RPMTAG_PREINPROG = 1085, /* s */
            RPMTAG_POSTINPROG = 1086, /* s */
            RPMTAG_PREUNPROG = 1087, /* s */
            RPMTAG_POSTUNPROG = 1088, /* s */
            RPMTAG_BUILDARCHS = 1089,
            RPMTAG_OBSOLETENAME = 1090, /* s[] */

            //#define RPMTAG_OBSOLETES RPMTAG_OBSOLETENAME    /* s[] */
            //#define RPMTAG_O        RPMTAG_OBSOLETENAME     /* s[] */
            RPMTAG_VERIFYSCRIPTPROG = 1091, /* s */

            RPMTAG_TRIGGERSCRIPTPROG = 1092, /* s */
            RPMTAG_DOCDIR = 1093,
            RPMTAG_COOKIE = 1094, /* s */
            RPMTAG_FILEDEVICES = 1095, /* i */
            RPMTAG_FILEINODES = 1096, /* i */
            RPMTAG_FILELANGS = 1097, /* s[] */
            RPMTAG_PREFIXES = 1098, /* s[] */
            RPMTAG_INSTPREFIXES = 1099, /* s[] */
            RPMTAG_TRIGGERIN = 1100,
            RPMTAG_TRIGGERUN = 1101,
            RPMTAG_TRIGGERPOSTUN = 1102,
            RPMTAG_AUTOREQ = 1103,
            RPMTAG_AUTOPROV = 1104,
            /*@-enummemuse@*/
            RPMTAG_CAPABILITY = 1105,
            /*@=enummemuse@*/
            RPMTAG_SOURCEPACKAGE = 1106,
            /*@-enummemuse@*/
            RPMTAG_OLDORIGFILENAMES = 1107,
            /*@=enummemuse@*/
            RPMTAG_BUILDPREREQ = 1108,
            RPMTAG_BUILDREQUIRES = 1109,
            RPMTAG_BUILDCONFLICTS = 1110,
            /*@-enummemuse@*/
            RPMTAG_BUILDMACROS = 1111,
            /*@=enummemuse@*/
            RPMTAG_PROVIDEFLAGS = 1112, /* i */
            RPMTAG_PROVIDEVERSION = 1113, /* s[] */
            RPMTAG_OBSOLETEFLAGS = 1114, /* i */
            RPMTAG_OBSOLETEVERSION = 1115, /* s[] */
            RPMTAG_DIRINDEXES = 1116, /* i */
            RPMTAG_BASENAMES = 1117, /* s[] */
            RPMTAG_DIRNAMES = 1118, /* s[] */
            RPMTAG_ORIGDIRINDEXES = 1119,
            RPMTAG_ORIGBASENAMES = 1120,
            RPMTAG_ORIGDIRNAMES = 1121,
            RPMTAG_OPTFLAGS = 1122, /* s */
            RPMTAG_DISTURL = 1123, /* s */
            RPMTAG_PAYLOADFORMAT = 1124, /* s */
            RPMTAG_PAYLOADCOMPRESSOR = 1125, /* s */
            RPMTAG_PAYLOADFLAGS = 1126, /* s */
            RPMTAG_INSTALLCOLOR = 1127,
            RPMTAG_INSTALLTID = 1128, /* i */
            RPMTAG_REMOVETID = 1129, /* i */
                                     /*@-enummemuse@*/
            RPMTAG_SHA1RHN = 1130,
            /*@=enummemuse@*/
            RPMTAG_RHNPLATFORM = 1131, /* s */
            RPMTAG_PLATFORM = 1132, /* s */
            RPMTAG_PATCHESNAME = 1133,
            RPMTAG_PATCHESFLAGS = 1134,
            RPMTAG_PATCHESVERSION = 1135,
            RPMTAG_CACHECTIME = 1136, /* i */
            RPMTAG_CACHEPKGPATH = 1137, /* s */
            RPMTAG_CACHEPKGSIZE = 1138, /* i */
            RPMTAG_CACHEPKGMTIME = 1139, /* i */
            RPMTAG_FILECOLORS = 1140, /* i */
            RPMTAG_FILECLASS = 1141, /* i */
            RPMTAG_CLASSDICT = 1142, /* s[] */
            RPMTAG_FILEDEPENDSX = 1143, /* i */
            RPMTAG_FILEDEPENDSN = 1144, /* i */
            RPMTAG_DEPENDSDICT = 1145, /* i */
            RPMTAG_SOURCEPKGID = 1146, /* x */
            RPMTAG_FILECONTEXTS = 1147, /* s[] */
            RPMTAG_FSCONTEXTS = 1148,
            RPMTAG_RECONTEXTS = 1149,
            RPMTAG_POLICIES = 1150,
            RPMTAG_PRETRANS = 1151, /* s */
            RPMTAG_POSTTRANS = 1152, /* s */
            RPMTAG_PRETRANSPROG = 1153, /* s */
            RPMTAG_POSTTRANSPROG = 1154, /* s */
            RPMTAG_DISTTAG = 1155, /* s */
            RPMTAG_SUGGESTSNAME = 1156, /* s[] extension placeholder */
            RPMTAG_SUGGESTSVERSION = 1157, /* s[] extension placeholder */
            RPMTAG_SUGGESTSFLAGS = 1158, /* i   extension placeholder */
            RPMTAG_ENHANCESNAME = 1159, /* s[] extension placeholder */
            RPMTAG_ENHANCESVERSION = 1160, /* s[] extension placeholder */
            RPMTAG_ENHANCESFLAGS = 1161, /* i   extension placeholder */
            RPMTAG_PRIORITY = 1162, /* i   extension placeholder */
            RPMTAG_CVSID = 1163, /* s */
                                 //#define RPMTAG_SVNID    RPMTAG_CVSID    /* s */

            /*@-enummemuse@*/
            RPMTAG_FIRSTFREE_TAG
            /*@=enummemuse@*/
        };

        public enum rpmSigTag
        {
            RPMSIGTAG_SIZE = 1000,
            RPMSIGTAG_LEMD5_1 = 1001,
            RPMSIGTAG_PGP = 1002,
            RPMSIGTAG_LEMD5_2 = 1003,
            RPMSIGTAG_MD5 = 1004,
            RPMSIGTAG_GPG = 1005,
            RPMSIGTAG_PGP5 = 1006,
            RPMSIGTAG_PAYLOADSIZE = 1007,
            RPMSIGTAG_RESERVEDSPACE = 1008,
            RPMSIGTAG_BADSHA1_1 = rpmTag.RPMTAG_BADSHA1_1,
            RPMSIGTAG_BADSHA1_2 = rpmTag.RPMTAG_BADSHA1_2,
            RPMSIGTAG_SHA1 = rpmTag.RPMTAG_SHA1HEADER,
            RPMSIGTAG_DSA = rpmTag.RPMTAG_DSAHEADER,
            RPMSIGTAG_RSA = rpmTag.RPMTAG_RSAHEADER,
            RPMSIGTAG_LONGSIZE = rpmTag.RPMTAG_LONGSIGSIZE,
            RPMSIGTAG_LONGARCHIVESIZE = rpmTag.RPMTAG_LONGARCHIVESIZE
        }
    }
}