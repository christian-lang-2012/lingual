using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lingual
{
    /// <summary>
    /// An enum of all the current languages supported for translation.
    /// </summary>
    public enum Locale
    {
        //Afrikaans locales
        af,
        af_ZA,
        
        //Arabic locales
        ar,
        ar_DZ,
        ar_BH,
        ar_EG,
        ar_IQ,
        ar_JO,
        ar_KW,
        ar_LB,
        ar_LY,
        ar_MA,
        ar_OM,
        ar_QA,
        ar_SA,
        ar_SY,
        ar_TN,
        ar_AE,
        ar_YE,
        
        //Belarusian locales
        be,
        be_BY,

        //Bulgarian locales
        bg,
        bg_BG,
        
        //Catalan locales
        ca,
        ca_ES,
        
        //Azeri (Cyrillic)
        cy,
        cy_AZ_AZ,

        //Serbian (Cyrillic)
        cy_sr_SP,

        //Uzbek (Cyrillic)
        cy_UZ_UZ,

        //Czech
        cs,
        cs_CZ,
        
        //Danish
        da,
        da_DK,
        
        //German locales
        de,
        de_AT,
        de_DE,
        de_LI,
        de_LU,
        de_CH,
        
        //Dhivehi
        div,
        div_MV,
        
        //Greek
        el,
        el_GR,
        
        //English locales
        en,
        en_AU,
        en_BZ,
        en_CA,
        en_CB,
        en_IE,
        en_JM,
        en_NZ,
        en_PH,
        en_ZA,
        en_TT,
        en_GB,
        en_US,
        en_ZW,

        //Spanish locales
        es,
        es_AR,
        es_BO,
        es_CL,
        es_CO,
        es_CR,
        es_DO,
        es_EC,
        es_SV,
        es_GT,
        es_HN,
        es_MX,
        es_NI,
        es_PA,
        es_PY,
        es_PE,
        es_PR,
        es_ES,
        es_UY,
        es_VE,
        
        //Estonia
        et,
        et_EE,

        //Basque
        eu,
        eu_ES,
        
        //Farsi
        fa,
        fa_IR,

        //Finnish
        fi,
        fi_FI,
        
        //Faroese
        fo,
        fo_FO,
        
        //French locales
        fr,
        fr_BE,
        fr_CA,
        fr_FR,
        fr_LU,
        fr_MC,
        fr_CH,
        
        //Hebrew
        he,
        he_IL,
        
        //Hindi
        hi,
        hi_IN,
        
        //Croatian
        hr,
        hr_HR,

        //Hungarian
        hu,
        hu_HU,
        
        //armenian
        hy,
        hy_AM,
        
        //Icelandic
        is_IS,

        //Indonesian
        id,
        id_ID,

        //Italian
        it,
        it_IT,
        it_CH,
        
        //Glacian
        gl,
        gl_ES,
        
        //Gujarati
        gu,
        gu_IN,

        //Japanese
        ja,
        ja_JP,
        
        //Georgian
        ka,
        ka_GE,
        
        //Kannada
        kn,
        kn_IN,
        
        //Kazakh
        kk,
        kk_KZ,
        
        //Konkani
        kok,
        kok_IN,
        
        //Korean
        ko,
        ko_KR,
        
        //Kyrgyz
        ky,
        ky_KZ,
        
        
        lt,

        //Azeri (Latin)
        lt_AZ_AZ,

        //Lithuanian
        lt_LT,

        //Serbian (Latin)
        lt_sr_SP,
        
        //Uzbek (Latin)
        lt_UZ_UZ,
        
        //Latvian
        lv,
        lv_LV,
          
        //Macedonian
        mk,
        mk_MK,
        
        //Malay
        ms,
        ms_BN,
        ms_MY,
        
        //Marathi
        mr,
        mr_IN,
        
        //Mongolian
        mn,
        mn_MN,
        
        //Norwegian
        nb,
        nb_NO,
        
        //Dutch
        nl,
        nl_BE,
        nl_NL,
        
        //Norwegian (Nynorsk)
        nn,
        nn_NO,
        
        //Punjabi
        pa,
        pa_IN,
        
        //Polish
        pl,
        pl_PL,
        
        //Portuguese
        pt,
        pt_BR,
        pt_PT,
        
        //Romanian
        ro,
        ro_RO,
        
        //Russian
        ru,
        ru_RU,
        
        //Sanskrit
        sa,
        sa_IN,
        
        //Slovak
        sk,
        sk_SK,
        
        //Slovenian
        sl,
        sl_SI,
        
        //Albanian
        sq,
        sq_AL,
        
        //Swahili
        sw,
        sw_KE,
        
        //Swedish
        sv,
        sv_FI,
        sv_SE,
        
        //Syriac
        syr,
        syr_SY,
        
        //Tamil
        ta,
        ta_IN,
        
        //Tatar
        tt,
        tt_RU,
        
        //Telugu
        te,
        te_IN,
        
        //Thai
        th,
        th_TH,
        
        //Turkish
        tr,
        tr_TR,
        
        //Ukrainian
        uk,
        uk_UA,
        
        //Urdu
        ur,
        ur_PK,
        
        //Vietnamese
        vi,
        vi_VN,
        
        //Chinese
        zh,
        zh_CN,
        zh_HK,
        zh_MO,
        zh_SG,
        zh_TW,
        zh_CHS,
        zh_CHT,
    }

    public static class LocaleMapper
    {
        public static Dictionary<Locale, Locale> LanguageToCultureMappings = new Dictionary<Locale,Locale>
        {
            {Locale.af, Locale.af},
            {Locale.af_ZA, Locale.af},
            {Locale.sq, Locale.sq},
            {Locale.sq_AL, Locale.sq},
            {Locale.ar, Locale.ar},
            {Locale.ar_DZ, Locale.ar},
            {Locale.ar_BH, Locale.ar},
            {Locale.ar_EG, Locale.ar},
            {Locale.ar_IQ, Locale.ar},
            {Locale.ar_JO, Locale.ar},
            {Locale.ar_KW, Locale.ar},
            {Locale.ar_LB, Locale.ar},
            {Locale.ar_LY, Locale.ar},
            {Locale.ar_MA, Locale.ar},
            {Locale.ar_OM, Locale.ar},
            {Locale.ar_QA, Locale.ar},
            {Locale.ar_SA, Locale.ar},
            {Locale.ar_SY, Locale.ar},
            {Locale.ar_TN, Locale.ar},
            {Locale.ar_AE, Locale.ar},
            {Locale.ar_YE, Locale.ar},
            {Locale.hy, Locale.hy},
            {Locale.hy_AM, Locale.hy},
            {Locale.cy, Locale.cy},
            {Locale.cy_AZ_AZ, Locale.cy},
            {Locale.lt, Locale.lt},
            {Locale.lt_AZ_AZ, Locale.lt},
            {Locale.eu, Locale.eu},
            {Locale.eu_ES, Locale.eu},
            {Locale.be, Locale.be},
            {Locale.be_BY, Locale.be},
            {Locale.bg, Locale.bg},
            {Locale.bg_BG, Locale.bg},
            {Locale.ca, Locale.ca},
            {Locale.ca_ES, Locale.ca},
            {Locale.zh, Locale.zh},
            {Locale.zh_CN, Locale.zh},
            {Locale.zh_HK, Locale.zh},
            {Locale.zh_MO, Locale.zh},
            {Locale.zh_SG, Locale.zh},
            {Locale.zh_TW, Locale.zh},
            {Locale.zh_CHS, Locale.zh},
            {Locale.zh_CHT, Locale.zh},
            {Locale.hr, Locale.hr},
            {Locale.hr_HR, Locale.hr},
            {Locale.cs, Locale.cs},
            {Locale.cs_CZ, Locale.cs},
            {Locale.da, Locale.da},
            {Locale.da_DK, Locale.da},
            {Locale.div, Locale.div},
            {Locale.div_MV, Locale.div},
            {Locale.nl, Locale.nl},
            {Locale.nl_BE, Locale.nl},
            {Locale.nl_NL, Locale.nl},
            {Locale.en, Locale.en},
            {Locale.en_AU, Locale.en},
            {Locale.en_BZ, Locale.en},
            {Locale.en_CA, Locale.en},
            {Locale.en_CB, Locale.en},
            {Locale.en_IE, Locale.en},
            {Locale.en_JM, Locale.en},
            {Locale.en_NZ, Locale.en},
            {Locale.en_PH, Locale.en},
            {Locale.en_ZA, Locale.en},
            {Locale.en_TT, Locale.en},
            {Locale.en_GB, Locale.en},
            {Locale.en_US, Locale.en},
            {Locale.en_ZW, Locale.en},
            {Locale.et, Locale.et},
            {Locale.et_EE, Locale.et},
            {Locale.fo, Locale.fo},
            {Locale.fo_FO, Locale.fo},
            {Locale.fa, Locale.fa},
            {Locale.fa_IR, Locale.fa},
            {Locale.fi, Locale.fi},
            {Locale.fi_FI, Locale.fi},
            {Locale.fr, Locale.fr},
            {Locale.fr_BE, Locale.fr},
            {Locale.fr_CA, Locale.fr},
            {Locale.fr_FR, Locale.fr},
            {Locale.fr_LU, Locale.fr},
            {Locale.fr_MC, Locale.fr},
            {Locale.fr_CH, Locale.fr},
            {Locale.gl, Locale.gl},
            {Locale.gl_ES, Locale.gl},
            {Locale.ka, Locale.ka},
            {Locale.ka_GE, Locale.ka},
            {Locale.de, Locale.de},
            {Locale.de_AT, Locale.de},
            {Locale.de_DE, Locale.de},
            {Locale.de_LI, Locale.de},
            {Locale.de_LU, Locale.de},
            {Locale.de_CH, Locale.de},
            {Locale.el, Locale.el},
            {Locale.el_GR, Locale.el},
            {Locale.gu, Locale.gu},
            {Locale.gu_IN, Locale.gu},
            {Locale.he, Locale.he},
            {Locale.he_IL, Locale.he},
            {Locale.hi, Locale.hi},
            {Locale.hi_IN, Locale.hi},
            {Locale.hu, Locale.hu},
            {Locale.hu_HU, Locale.hu},
            {Locale.is_IS, Locale.is_IS},
            {Locale.id, Locale.id},
            {Locale.id_ID, Locale.id},
            {Locale.it, Locale.it},
            {Locale.it_IT, Locale.it},
            {Locale.it_CH, Locale.it},
            {Locale.ja, Locale.ja},
            {Locale.ja_JP, Locale.ja},
            {Locale.kn, Locale.kn},
            {Locale.kn_IN, Locale.kn},
            {Locale.kk, Locale.kk},
            {Locale.kk_KZ, Locale.kk},
            {Locale.kok, Locale.kok},
            {Locale.kok_IN, Locale.kok},
            {Locale.ko, Locale.ko},
            {Locale.ko_KR, Locale.ko},
            {Locale.ky, Locale.ky},
            {Locale.ky_KZ, Locale.ky},
            {Locale.lv, Locale.lv},
            {Locale.lv_LV, Locale.lv},
            {Locale.lt_LT, Locale.lt},
            {Locale.mk, Locale.mk},
            {Locale.mk_MK, Locale.mk},
            {Locale.ms, Locale.ms},
            {Locale.ms_BN, Locale.ms},
            {Locale.ms_MY, Locale.ms},
            {Locale.mr, Locale.mr},
            {Locale.mr_IN, Locale.mr},
            {Locale.mn, Locale.mn},
            {Locale.mn_MN, Locale.mn},
            {Locale.nb, Locale.nb},
            {Locale.nb_NO, Locale.nb},
            {Locale.nn, Locale.nn},
            {Locale.nn_NO, Locale.nn},
            {Locale.pl, Locale.pl},
            {Locale.pl_PL, Locale.pl},
            {Locale.pt, Locale.pt},
            {Locale.pt_BR, Locale.pt},
            {Locale.pt_PT, Locale.pt},
            {Locale.pa, Locale.pa},
            {Locale.pa_IN, Locale.pa},
            {Locale.ro, Locale.ro},
            {Locale.ro_RO, Locale.ro},
            {Locale.ru, Locale.ru},
            {Locale.ru_RU, Locale.ru},
            {Locale.sa, Locale.sa},
            {Locale.sa_IN, Locale.sa},
            {Locale.cy_sr_SP, Locale.cy},
            {Locale.lt_sr_SP, Locale.lt},
            {Locale.sk, Locale.sk},
            {Locale.sk_SK, Locale.sk},
            {Locale.sl, Locale.sl},
            {Locale.sl_SI, Locale.sl},
            {Locale.es, Locale.es},
            {Locale.es_AR, Locale.es},
            {Locale.es_BO, Locale.es},
            {Locale.es_CL, Locale.es},
            {Locale.es_CO, Locale.es},
            {Locale.es_CR, Locale.es},
            {Locale.es_DO, Locale.es},
            {Locale.es_EC, Locale.es},
            {Locale.es_SV, Locale.es},
            {Locale.es_GT, Locale.es},
            {Locale.es_HN, Locale.es},
            {Locale.es_MX, Locale.es},
            {Locale.es_NI, Locale.es},
            {Locale.es_PA, Locale.es},
            {Locale.es_PY, Locale.es},
            {Locale.es_PE, Locale.es},
            {Locale.es_PR, Locale.es},
            {Locale.es_ES, Locale.es},
            {Locale.es_UY, Locale.es},
            {Locale.es_VE, Locale.es},
            {Locale.sw, Locale.sw},
            {Locale.sw_KE, Locale.sw},
            {Locale.sv, Locale.sv},
            {Locale.sv_FI, Locale.sv},
            {Locale.sv_SE, Locale.sv},
            {Locale.syr, Locale.syr},
            {Locale.syr_SY, Locale.syr},
            {Locale.ta, Locale.ta},
            {Locale.ta_IN, Locale.ta},
            {Locale.tt, Locale.tt},
            {Locale.tt_RU, Locale.tt},
            {Locale.te, Locale.te},
            {Locale.te_IN, Locale.te},
            {Locale.th, Locale.th},
            {Locale.th_TH, Locale.th},
            {Locale.tr, Locale.tr},
            {Locale.tr_TR, Locale.tr},
            {Locale.uk, Locale.uk},
            {Locale.uk_UA, Locale.uk},
            {Locale.ur, Locale.ur},
            {Locale.ur_PK, Locale.ur},
            {Locale.cy_UZ_UZ, Locale.cy},
            {Locale.lt_UZ_UZ, Locale.lt},
            {Locale.vi, Locale.vi},
            {Locale.vi_VN, Locale.vi}
        };
    }
}
