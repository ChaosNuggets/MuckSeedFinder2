using System;
using System.Collections.Generic;
using UnityEngine;

public static class HeightCurve
{
    const int DECIMAL_PLACES = 3;

    public static float Evaluate(float f)
    {
        f *= Mathf.Pow(10f, DECIMAL_PLACES);
        int f_ceil = Mathf.CeilToInt(f);
        int f_floor = Mathf.FloorToInt(f);

        return Mathf.Lerp(heightCurve[f_floor], heightCurve[f_ceil], f - f_floor);
    }

    // I give up on trying to recreate the height curve so this is what I'm going to do instead (these values are x1000)
    private static readonly Dictionary<int, float> heightCurve = new()
    {
        {0, 0f},
        {1, 2.203752E-06f},
        {2, 8.803047E-06f},
        {3, 1.977994E-05f},
        {4, 3.51165E-05f},
        {5, 5.479476E-05f},
        {6, 7.879679E-05f},
        {7, 0.0001071047f},
        {8, 0.0001397004f},
        {9, 0.0001765661f},
        {10, 0.0002176838f},
        {11, 0.0002630356f},
        {12, 0.0003126034f},
        {13, 0.0003663695f},
        {14, 0.0004243158f},
        {15, 0.0004864243f},
        {16, 0.0005526773f},
        {17, 0.0006230566f},
        {18, 0.0006975443f},
        {19, 0.0007761226f},
        {20, 0.0008587734f},
        {21, 0.0009454789f},
        {22, 0.001036221f},
        {23, 0.001130982f},
        {24, 0.001229744f},
        {25, 0.001332488f},
        {26, 0.001439198f},
        {27, 0.001549854f},
        {28, 0.00166444f},
        {29, 0.001782937f},
        {30, 0.001905326f},
        {31, 0.002031591f},
        {32, 0.002161714f},
        {33, 0.002295675f},
        {34, 0.002433458f},
        {35, 0.002575044f},
        {36, 0.002720416f},
        {37, 0.002869555f},
        {38, 0.003022444f},
        {39, 0.003179065f},
        {40, 0.003339399f},
        {41, 0.00350343f},
        {42, 0.003671138f},
        {43, 0.003842506f},
        {44, 0.004017515f},
        {45, 0.00419615f},
        {46, 0.004378389f},
        {47, 0.004564216f},
        {48, 0.004753615f},
        {49, 0.004946566f},
        {50, 0.005143051f},
        {51, 0.005343051f},
        {52, 0.005546551f},
        {53, 0.005753531f},
        {54, 0.005963973f},
        {55, 0.006177861f},
        {56, 0.006395175f},
        {57, 0.006615897f},
        {58, 0.00684001f},
        {59, 0.007067495f},
        {60, 0.007298336f},
        {61, 0.007532515f},
        {62, 0.007770011f},
        {63, 0.008010809f},
        {64, 0.008254891f},
        {65, 0.008502235f},
        {66, 0.008752829f},
        {67, 0.009006651f},
        {68, 0.009263687f},
        {69, 0.009523912f},
        {70, 0.009787316f},
        {71, 0.01005387f},
        {72, 0.01032357f},
        {73, 0.0105964f},
        {74, 0.01087232f},
        {75, 0.01115133f},
        {76, 0.01143341f},
        {77, 0.01171854f},
        {78, 0.0120067f},
        {79, 0.01229787f},
        {80, 0.01259204f},
        {81, 0.01288919f},
        {82, 0.0131893f},
        {83, 0.01349235f},
        {84, 0.01379832f},
        {85, 0.01410721f},
        {86, 0.01441898f},
        {87, 0.01473362f},
        {88, 0.01505111f},
        {89, 0.01537144f},
        {90, 0.01569458f},
        {91, 0.01602052f},
        {92, 0.01634924f},
        {93, 0.01668073f},
        {94, 0.01701496f},
        {95, 0.01735192f},
        {96, 0.01769158f},
        {97, 0.01803394f},
        {98, 0.01837897f},
        {99, 0.01872666f},
        {100, 0.01907698f},
        {101, 0.01942992f},
        {102, 0.01978546f},
        {103, 0.02014359f},
        {104, 0.02050428f},
        {105, 0.02086752f},
        {106, 0.02123329f},
        {107, 0.02160157f},
        {108, 0.02197234f},
        {109, 0.02234559f},
        {110, 0.0227213f},
        {111, 0.02309945f},
        {112, 0.02348002f},
        {113, 0.02386299f},
        {114, 0.02424835f},
        {115, 0.02463607f},
        {116, 0.02502615f},
        {117, 0.02541856f},
        {118, 0.02581328f},
        {119, 0.0262103f},
        {120, 0.0266096f},
        {121, 0.02701116f},
        {122, 0.02741496f},
        {123, 0.02782098f},
        {124, 0.02822921f},
        {125, 0.02863964f},
        {126, 0.02905223f},
        {127, 0.02946697f},
        {128, 0.02988385f},
        {129, 0.03030284f},
        {130, 0.03072393f},
        {131, 0.03114711f},
        {132, 0.03157235f},
        {133, 0.03199963f},
        {134, 0.03242895f},
        {135, 0.03286026f},
        {136, 0.03329358f},
        {137, 0.03372885f},
        {138, 0.03416609f},
        {139, 0.03460527f},
        {140, 0.03504637f},
        {141, 0.03548937f},
        {142, 0.03593425f},
        {143, 0.036381f},
        {144, 0.03682958f},
        {145, 0.03728001f},
        {146, 0.03773225f},
        {147, 0.03818627f},
        {148, 0.03864208f},
        {149, 0.03909965f},
        {150, 0.03955895f},
        {151, 0.04001997f},
        {152, 0.0404827f},
        {153, 0.04094712f},
        {154, 0.0414132f},
        {155, 0.04188094f},
        {156, 0.04235031f},
        {157, 0.04282129f},
        {158, 0.04329387f},
        {159, 0.04376802f},
        {160, 0.04424373f},
        {161, 0.044721f},
        {162, 0.04519979f},
        {163, 0.04568007f},
        {164, 0.04616186f},
        {165, 0.04664511f},
        {166, 0.04712981f},
        {167, 0.04761594f},
        {168, 0.0481035f},
        {169, 0.04859246f},
        {170, 0.0490828f},
        {171, 0.0495745f},
        {172, 0.05006754f},
        {173, 0.0505619f},
        {174, 0.05105759f},
        {175, 0.05155456f},
        {176, 0.05205281f},
        {177, 0.05255231f},
        {178, 0.05305305f},
        {179, 0.05355502f},
        {180, 0.05405818f},
        {181, 0.05456252f},
        {182, 0.05506802f},
        {183, 0.05557469f},
        {184, 0.05608248f},
        {185, 0.05659138f},
        {186, 0.05710137f},
        {187, 0.05761244f},
        {188, 0.05812456f},
        {189, 0.05863774f},
        {190, 0.05915193f},
        {191, 0.05966713f},
        {192, 0.06018331f},
        {193, 0.06070046f},
        {194, 0.06121856f},
        {195, 0.0617376f},
        {196, 0.06225754f},
        {197, 0.06277839f},
        {198, 0.06330011f},
        {199, 0.06382269f},
        {200, 0.06434613f},
        {201, 0.06487038f},
        {202, 0.06539544f},
        {203, 0.06592128f},
        {204, 0.06644791f},
        {205, 0.06697528f},
        {206, 0.06750339f},
        {207, 0.06803221f},
        {208, 0.06856174f},
        {209, 0.06909195f},
        {210, 0.06962281f},
        {211, 0.07015433f},
        {212, 0.07068647f},
        {213, 0.07121923f},
        {214, 0.07175257f},
        {215, 0.0722865f},
        {216, 0.07282097f},
        {217, 0.07335597f},
        {218, 0.07389151f},
        {219, 0.07442755f},
        {220, 0.07496405f},
        {221, 0.07550105f},
        {222, 0.07603848f},
        {223, 0.07657634f},
        {224, 0.07711462f},
        {225, 0.07765328f},
        {226, 0.07819232f},
        {227, 0.07873173f},
        {228, 0.07927147f},
        {229, 0.07981155f},
        {230, 0.08035191f},
        {231, 0.08089257f},
        {232, 0.0814335f},
        {233, 0.08197664f},
        {234, 0.0825306f},
        {235, 0.08309619f},
        {236, 0.08367325f},
        {237, 0.08426164f},
        {238, 0.08486117f},
        {239, 0.0854717f},
        {240, 0.08609308f},
        {241, 0.08672515f},
        {242, 0.08736773f},
        {243, 0.08802067f},
        {244, 0.08868383f},
        {245, 0.08935703f},
        {246, 0.09004012f},
        {247, 0.09073292f},
        {248, 0.09143531f},
        {249, 0.0921471f},
        {250, 0.09286815f},
        {251, 0.09359828f},
        {252, 0.09433737f},
        {253, 0.09508521f},
        {254, 0.09584169f},
        {255, 0.0966066f},
        {256, 0.09737985f},
        {257, 0.09816121f},
        {258, 0.09895054f},
        {259, 0.09974772f},
        {260, 0.1005526f},
        {261, 0.1013649f},
        {262, 0.1021846f},
        {263, 0.1030115f},
        {264, 0.1038454f},
        {265, 0.1046861f},
        {266, 0.1055336f},
        {267, 0.1063877f},
        {268, 0.1072481f},
        {269, 0.1081148f},
        {270, 0.1089875f},
        {271, 0.1098662f},
        {272, 0.1107506f},
        {273, 0.1116406f},
        {274, 0.1125361f},
        {275, 0.1134369f},
        {276, 0.1143427f},
        {277, 0.1152536f},
        {278, 0.1161692f},
        {279, 0.1170895f},
        {280, 0.1180143f},
        {281, 0.1189434f},
        {282, 0.1198767f},
        {283, 0.1208139f},
        {284, 0.1217551f},
        {285, 0.1226999f},
        {286, 0.1236483f},
        {287, 0.1246f},
        {288, 0.125555f},
        {289, 0.126513f},
        {290, 0.127474f},
        {291, 0.1284376f},
        {292, 0.1294039f},
        {293, 0.1303726f},
        {294, 0.1313435f},
        {295, 0.1323165f},
        {296, 0.1332915f},
        {297, 0.1342683f},
        {298, 0.1352468f},
        {299, 0.1362266f},
        {300, 0.1372078f},
        {301, 0.1381902f},
        {302, 0.1391735f},
        {303, 0.1401577f},
        {304, 0.1411426f},
        {305, 0.142128f},
        {306, 0.1431137f},
        {307, 0.1440997f},
        {308, 0.1450856f},
        {309, 0.1460715f},
        {310, 0.1470571f},
        {311, 0.1480422f},
        {312, 0.1490268f},
        {313, 0.1500106f},
        {314, 0.1509935f},
        {315, 0.1519753f},
        {316, 0.1529559f},
        {317, 0.1539352f},
        {318, 0.1549128f},
        {319, 0.1558888f},
        {320, 0.1568629f},
        {321, 0.157835f},
        {322, 0.1588049f},
        {323, 0.1597725f},
        {324, 0.1607375f},
        {325, 0.1616999f},
        {326, 0.1626595f},
        {327, 0.1636161f},
        {328, 0.1645696f},
        {329, 0.1655198f},
        {330, 0.1664665f},
        {331, 0.1674096f},
        {332, 0.168349f},
        {333, 0.1692844f},
        {334, 0.1702158f},
        {335, 0.1711428f},
        {336, 0.1720655f},
        {337, 0.1729837f},
        {338, 0.1738971f},
        {339, 0.1748056f},
        {340, 0.1757091f},
        {341, 0.1766074f},
        {342, 0.1775003f},
        {343, 0.1783877f},
        {344, 0.1792695f},
        {345, 0.1801454f},
        {346, 0.1810153f},
        {347, 0.1818791f},
        {348, 0.1827365f},
        {349, 0.1835875f},
        {350, 0.1844319f},
        {351, 0.1852695f},
        {352, 0.1861001f},
        {353, 0.1869236f},
        {354, 0.1877399f},
        {355, 0.1885487f},
        {356, 0.18935f},
        {357, 0.1901435f},
        {358, 0.1909292f},
        {359, 0.1917067f},
        {360, 0.1924761f},
        {361, 0.1932371f},
        {362, 0.1939895f},
        {363, 0.1947333f},
        {364, 0.1954682f},
        {365, 0.1961941f},
        {366, 0.1969108f},
        {367, 0.1976182f},
        {368, 0.1983161f},
        {369, 0.1990044f},
        {370, 0.1996829f},
        {371, 0.2003514f},
        {372, 0.2010098f},
        {373, 0.2016579f},
        {374, 0.2022956f},
        {375, 0.2029226f},
        {376, 0.2035389f},
        {377, 0.2041443f},
        {378, 0.2047387f},
        {379, 0.2053218f},
        {380, 0.2058952f},
        {381, 0.2064672f},
        {382, 0.2070388f},
        {383, 0.2076099f},
        {384, 0.2081805f},
        {385, 0.2087507f},
        {386, 0.2093204f},
        {387, 0.2098897f},
        {388, 0.2104584f},
        {389, 0.2110266f},
        {390, 0.2115944f},
        {391, 0.2121616f},
        {392, 0.2127283f},
        {393, 0.2132945f},
        {394, 0.2138601f},
        {395, 0.2144252f},
        {396, 0.2149898f},
        {397, 0.2155538f},
        {398, 0.2161172f},
        {399, 0.21668f},
        {400, 0.2172423f},
        {401, 0.2178039f},
        {402, 0.218365f},
        {403, 0.2189255f},
        {404, 0.2194853f},
        {405, 0.2200446f},
        {406, 0.2206032f},
        {407, 0.2211612f},
        {408, 0.2217185f},
        {409, 0.2222751f},
        {410, 0.2228311f},
        {411, 0.2233865f},
        {412, 0.2239412f},
        {413, 0.2244951f},
        {414, 0.2250484f},
        {415, 0.225601f},
        {416, 0.2261529f},
        {417, 0.2267041f},
        {418, 0.2272545f},
        {419, 0.2278042f},
        {420, 0.2283532f},
        {421, 0.2289015f},
        {422, 0.2294489f},
        {423, 0.2299957f},
        {424, 0.2305416f},
        {425, 0.2310868f},
        {426, 0.2316312f},
        {427, 0.2321748f},
        {428, 0.2327176f},
        {429, 0.2332596f},
        {430, 0.2338007f},
        {431, 0.2343411f},
        {432, 0.2348806f},
        {433, 0.2354193f},
        {434, 0.2359571f},
        {435, 0.2364941f},
        {436, 0.2370302f},
        {437, 0.2375655f},
        {438, 0.2380998f},
        {439, 0.2386333f},
        {440, 0.2391659f},
        {441, 0.2396976f},
        {442, 0.2402284f},
        {443, 0.2407582f},
        {444, 0.2412872f},
        {445, 0.2418151f},
        {446, 0.2423422f},
        {447, 0.2428683f},
        {448, 0.2433935f},
        {449, 0.2439177f},
        {450, 0.2444409f},
        {451, 0.2449632f},
        {452, 0.2454844f},
        {453, 0.2460047f},
        {454, 0.2465239f},
        {455, 0.2470422f},
        {456, 0.2475594f},
        {457, 0.2480756f},
        {458, 0.2485908f},
        {459, 0.2491049f},
        {460, 0.2496181f},
        {461, 0.2501301f},
        {462, 0.250641f},
        {463, 0.2511509f},
        {464, 0.2516597f},
        {465, 0.2521675f},
        {466, 0.2526741f},
        {467, 0.2531796f},
        {468, 0.253684f},
        {469, 0.2541873f},
        {470, 0.2546895f},
        {471, 0.2551905f},
        {472, 0.2556904f},
        {473, 0.2561891f},
        {474, 0.2566867f},
        {475, 0.2571831f},
        {476, 0.2576784f},
        {477, 0.2581725f},
        {478, 0.2586653f},
        {479, 0.259157f},
        {480, 0.2596475f},
        {481, 0.2601367f},
        {482, 0.2606248f},
        {483, 0.2611116f},
        {484, 0.2615972f},
        {485, 0.2620815f},
        {486, 0.2625646f},
        {487, 0.2630464f},
        {488, 0.2635269f},
        {489, 0.2640062f},
        {490, 0.2644842f},
        {491, 0.2649609f},
        {492, 0.2654364f},
        {493, 0.2659104f},
        {494, 0.2663832f},
        {495, 0.2668547f},
        {496, 0.2673248f},
        {497, 0.2677937f},
        {498, 0.2682611f},
        {499, 0.2687272f},
        {500, 0.269192f},
        {501, 0.2696553f},
        {502, 0.2701173f},
        {503, 0.270578f},
        {504, 0.2710372f},
        {505, 0.271495f},
        {506, 0.2719514f},
        {507, 0.2724065f},
        {508, 0.27286f},
        {509, 0.2733122f},
        {510, 0.2737629f},
        {511, 0.2742122f},
        {512, 0.27466f},
        {513, 0.2751063f},
        {514, 0.2755512f},
        {515, 0.2759946f},
        {516, 0.2764366f},
        {517, 0.276877f},
        {518, 0.2773159f},
        {519, 0.2777534f},
        {520, 0.2781893f},
        {521, 0.2786237f},
        {522, 0.2790565f},
        {523, 0.2794878f},
        {524, 0.2799176f},
        {525, 0.2803458f},
        {526, 0.2807725f},
        {527, 0.2811975f},
        {528, 0.2816211f},
        {529, 0.282043f},
        {530, 0.2824633f},
        {531, 0.282882f},
        {532, 0.2832991f},
        {533, 0.2837147f},
        {534, 0.2841285f},
        {535, 0.2845408f},
        {536, 0.2849514f},
        {537, 0.2853603f},
        {538, 0.2857676f},
        {539, 0.2861732f},
        {540, 0.2865772f},
        {541, 0.2869795f},
        {542, 0.2873801f},
        {543, 0.287779f},
        {544, 0.2881762f},
        {545, 0.2885717f},
        {546, 0.2889655f},
        {547, 0.2893575f},
        {548, 0.2897478f},
        {549, 0.2901364f},
        {550, 0.2905232f},
        {551, 0.2909083f},
        {552, 0.2912916f},
        {553, 0.2916731f},
        {554, 0.2920529f},
        {555, 0.2924308f},
        {556, 0.292807f},
        {557, 0.2931814f},
        {558, 0.2935539f},
        {559, 0.2939247f},
        {560, 0.2942936f},
        {561, 0.2946606f},
        {562, 0.2950259f},
        {563, 0.2953892f},
        {564, 0.2957508f},
        {565, 0.2961104f},
        {566, 0.2964682f},
        {567, 0.2968241f},
        {568, 0.2971781f},
        {569, 0.2975302f},
        {570, 0.2978804f},
        {571, 0.2982287f},
        {572, 0.2985751f},
        {573, 0.2989196f},
        {574, 0.2992621f},
        {575, 0.2996027f},
        {576, 0.2999413f},
        {577, 0.3002779f},
        {578, 0.3006126f},
        {579, 0.3009453f},
        {580, 0.3012761f},
        {581, 0.3016048f},
        {582, 0.3019316f},
        {583, 0.3022563f},
        {584, 0.3025791f},
        {585, 0.3028998f},
        {586, 0.3032366f},
        {587, 0.3036134f},
        {588, 0.3040298f},
        {589, 0.3044855f},
        {590, 0.3049801f},
        {591, 0.3055132f},
        {592, 0.3060844f},
        {593, 0.3066933f},
        {594, 0.3073395f},
        {595, 0.3080227f},
        {596, 0.3087425f},
        {597, 0.3094983f},
        {598, 0.31029f},
        {599, 0.311117f},
        {600, 0.3119791f},
        {601, 0.3128757f},
        {602, 0.3138065f},
        {603, 0.3147711f},
        {604, 0.3157692f},
        {605, 0.3168004f},
        {606, 0.3178641f},
        {607, 0.3189602f},
        {608, 0.3200881f},
        {609, 0.3212475f},
        {610, 0.322438f},
        {611, 0.3236592f},
        {612, 0.3249107f},
        {613, 0.3261921f},
        {614, 0.3275031f},
        {615, 0.3288432f},
        {616, 0.3302121f},
        {617, 0.3316093f},
        {618, 0.3330345f},
        {619, 0.3344874f},
        {620, 0.3359674f},
        {621, 0.3374742f},
        {622, 0.3390074f},
        {623, 0.3405667f},
        {624, 0.3421516f},
        {625, 0.3437617f},
        {626, 0.3453967f},
        {627, 0.3470562f},
        {628, 0.3487398f},
        {629, 0.3504471f},
        {630, 0.3521776f},
        {631, 0.353931f},
        {632, 0.3557071f},
        {633, 0.3575052f},
        {634, 0.3593251f},
        {635, 0.3611663f},
        {636, 0.3630284f},
        {637, 0.3649113f},
        {638, 0.3668142f},
        {639, 0.3687369f},
        {640, 0.3706791f},
        {641, 0.3726402f},
        {642, 0.3746201f},
        {643, 0.3766181f},
        {644, 0.3786339f},
        {645, 0.3806673f},
        {646, 0.3827178f},
        {647, 0.3847848f},
        {648, 0.3868681f},
        {649, 0.3889674f},
        {650, 0.3910822f},
        {651, 0.3932122f},
        {652, 0.3953568f},
        {653, 0.3975157f},
        {654, 0.3996886f},
        {655, 0.4018751f},
        {656, 0.4040748f},
        {657, 0.4062873f},
        {658, 0.4085121f},
        {659, 0.4107489f},
        {660, 0.4129975f},
        {661, 0.4152571f},
        {662, 0.4175276f},
        {663, 0.4198085f},
        {664, 0.4220995f},
        {665, 0.4244003f},
        {666, 0.4267103f},
        {667, 0.4290291f},
        {668, 0.4313564f},
        {669, 0.433692f},
        {670, 0.4360351f},
        {671, 0.4383856f},
        {672, 0.440743f},
        {673, 0.443107f},
        {674, 0.4454772f},
        {675, 0.4478531f},
        {676, 0.4502344f},
        {677, 0.4526207f},
        {678, 0.4550115f},
        {679, 0.4574068f},
        {680, 0.4598057f},
        {681, 0.462208f},
        {682, 0.4646134f},
        {683, 0.4670216f},
        {684, 0.4694318f},
        {685, 0.471844f},
        {686, 0.4742576f},
        {687, 0.4766724f},
        {688, 0.479088f},
        {689, 0.4815038f},
        {690, 0.4839195f},
        {691, 0.4863347f},
        {692, 0.4887491f},
        {693, 0.4911624f},
        {694, 0.4935739f},
        {695, 0.4959834f},
        {696, 0.4983905f},
        {697, 0.5007949f},
        {698, 0.503196f},
        {699, 0.5055935f},
        {700, 0.507987f},
        {701, 0.5103762f},
        {702, 0.5127608f},
        {703, 0.5151401f},
        {704, 0.5175138f},
        {705, 0.5198817f},
        {706, 0.5222432f},
        {707, 0.5245982f},
        {708, 0.5269459f},
        {709, 0.5292862f},
        {710, 0.5316187f},
        {711, 0.5339429f},
        {712, 0.5362585f},
        {713, 0.538565f},
        {714, 0.540862f},
        {715, 0.5431493f},
        {716, 0.5454264f},
        {717, 0.5476929f},
        {718, 0.5499483f},
        {719, 0.5521924f},
        {720, 0.554425f},
        {721, 0.5566452f},
        {722, 0.5588528f},
        {723, 0.5610475f},
        {724, 0.5632289f},
        {725, 0.5653967f},
        {726, 0.5675503f},
        {727, 0.5696894f},
        {728, 0.5718136f},
        {729, 0.5739226f},
        {730, 0.576016f},
        {731, 0.5780933f},
        {732, 0.5801541f},
        {733, 0.5821981f},
        {734, 0.584225f},
        {735, 0.5862342f},
        {736, 0.5882254f},
        {737, 0.5901982f},
        {738, 0.5921522f},
        {739, 0.5940871f},
        {740, 0.5960025f},
        {741, 0.5978978f},
        {742, 0.5997728f},
        {743, 0.6016272f},
        {744, 0.6034604f},
        {745, 0.6052722f},
        {746, 0.607062f},
        {747, 0.6088295f},
        {748, 0.6105745f},
        {749, 0.6122962f},
        {750, 0.6139946f},
        {751, 0.6156691f},
        {752, 0.6173195f},
        {753, 0.6189452f},
        {754, 0.6205459f},
        {755, 0.6221211f},
        {756, 0.6236706f},
        {757, 0.625194f},
        {758, 0.6266907f},
        {759, 0.6281605f},
        {760, 0.629603f},
        {761, 0.6310177f},
        {762, 0.6324043f},
        {763, 0.6337624f},
        {764, 0.6350915f},
        {765, 0.6363914f},
        {766, 0.6376616f},
        {767, 0.6389018f},
        {768, 0.6401114f},
        {769, 0.6412903f},
        {770, 0.6424378f},
        {771, 0.6435539f},
        {772, 0.6446378f},
        {773, 0.6456893f},
        {774, 0.646708f},
        {775, 0.6476935f},
        {776, 0.6486455f},
        {777, 0.6495635f},
        {778, 0.6504471f},
        {779, 0.651296f},
        {780, 0.6521097f},
        {781, 0.652888f},
        {782, 0.6536303f},
        {783, 0.6543363f},
        {784, 0.6550056f},
        {785, 0.6556379f},
        {786, 0.6562326f},
        {787, 0.6567894f},
        {788, 0.657308f},
        {789, 0.657788f},
        {790, 0.658229f},
        {791, 0.6586305f},
        {792, 0.6589923f},
        {793, 0.6593138f},
        {794, 0.6595948f},
        {795, 0.6598346f},
        {796, 0.6600332f},
        {797, 0.66019f},
        {798, 0.6603047f},
        {799, 0.6603768f},
        {800, 0.6604061f},
        {801, 0.6604067f},
        {802, 0.6604067f},
        {803, 0.6604067f},
        {804, 0.6604067f},
        {805, 0.6604067f},
        {806, 0.6604067f},
        {807, 0.6604067f},
        {808, 0.6604067f},
        {809, 0.6604067f},
        {810, 0.6604067f},
        {811, 0.6604067f},
        {812, 0.6604067f},
        {813, 0.6604067f},
        {814, 0.6604067f},
        {815, 0.6604067f},
        {816, 0.6604067f},
        {817, 0.6604067f},
        {818, 0.6604067f},
        {819, 0.6604067f},
        {820, 0.6604067f},
        {821, 0.6604067f},
        {822, 0.6604067f},
        {823, 0.6604067f},
        {824, 0.6604067f},
        {825, 0.6604067f},
        {826, 0.6604067f},
        {827, 0.6604067f},
        {828, 0.6604067f},
        {829, 0.6604067f},
        {830, 0.6604067f},
        {831, 0.6604067f},
        {832, 0.6604067f},
        {833, 0.6604067f},
        {834, 0.6604067f},
        {835, 0.6604067f},
        {836, 0.6604067f},
        {837, 0.6604067f},
        {838, 0.6604067f},
        {839, 0.6604067f},
        {840, 0.6604067f},
        {841, 0.6604067f},
        {842, 0.6604067f},
        {843, 0.6604067f},
        {844, 0.6604067f},
        {845, 0.6604067f},
        {846, 0.6604067f},
        {847, 0.6604067f},
        {848, 0.6604067f},
        {849, 0.6604067f},
        {850, 0.6604067f},
        {851, 0.6604067f},
        {852, 0.6604067f},
        {853, 0.6604067f},
        {854, 0.6604067f},
        {855, 0.6604067f},
        {856, 0.6604067f},
        {857, 0.6604067f},
        {858, 0.6604067f},
        {859, 0.6604067f},
        {860, 0.6604067f},
        {861, 0.6604067f},
        {862, 0.6604067f},
        {863, 0.6604067f},
        {864, 0.6604067f},
        {865, 0.6604067f},
        {866, 0.6604067f},
        {867, 0.6604067f},
        {868, 0.6604067f},
        {869, 0.6604067f},
        {870, 0.6604067f},
        {871, 0.6604067f},
        {872, 0.6604067f},
        {873, 0.6604067f},
        {874, 0.6604067f},
        {875, 0.6604067f},
        {876, 0.6604067f},
        {877, 0.6604067f},
        {878, 0.6604067f},
        {879, 0.6604067f},
        {880, 0.6604067f},
        {881, 0.6604067f},
        {882, 0.6604067f},
        {883, 0.6604067f},
        {884, 0.6604067f},
        {885, 0.6604067f},
        {886, 0.6604067f},
        {887, 0.6604067f},
        {888, 0.6604067f},
        {889, 0.6604067f},
        {890, 0.6604067f},
        {891, 0.6604067f},
        {892, 0.6604067f},
        {893, 0.6604067f},
        {894, 0.6604067f},
        {895, 0.6604067f},
        {896, 0.6604067f},
        {897, 0.6604067f},
        {898, 0.6604067f},
        {899, 0.6604067f},
        {900, 0.6604067f},
        {901, 0.6604067f},
        {902, 0.6604067f},
        {903, 0.6604067f},
        {904, 0.6604067f},
        {905, 0.6604067f},
        {906, 0.6604067f},
        {907, 0.6604067f},
        {908, 0.6604067f},
        {909, 0.6604067f},
        {910, 0.6604067f},
        {911, 0.6604067f},
        {912, 0.6604067f},
        {913, 0.6604067f},
        {914, 0.6604067f},
        {915, 0.6604067f},
        {916, 0.6604067f},
        {917, 0.6604067f},
        {918, 0.6604067f},
        {919, 0.6604067f},
        {920, 0.6604067f},
        {921, 0.6604067f},
        {922, 0.6604067f},
        {923, 0.6604067f},
        {924, 0.6604067f},
        {925, 0.6604067f},
        {926, 0.6604067f},
        {927, 0.6604067f},
        {928, 0.6604067f},
        {929, 0.6604067f},
        {930, 0.6604067f},
        {931, 0.6604067f},
        {932, 0.6604067f},
        {933, 0.6604067f},
        {934, 0.6604067f},
        {935, 0.6604067f},
        {936, 0.6604067f},
        {937, 0.6604067f},
        {938, 0.6604067f},
        {939, 0.6604067f},
        {940, 0.6604067f},
        {941, 0.6604067f},
        {942, 0.6604067f},
        {943, 0.6604067f},
        {944, 0.6604067f},
        {945, 0.6604067f},
        {946, 0.6604067f},
        {947, 0.6604067f},
        {948, 0.6604067f},
        {949, 0.6604067f},
        {950, 0.6604067f},
        {951, 0.6604067f},
        {952, 0.6604067f},
        {953, 0.6604067f},
        {954, 0.6604067f},
        {955, 0.6604067f},
        {956, 0.6604067f},
        {957, 0.6604067f},
        {958, 0.6604067f},
        {959, 0.6604067f},
        {960, 0.6604067f},
        {961, 0.6604067f},
        {962, 0.6604067f},
        {963, 0.6604067f},
        {964, 0.6604067f},
        {965, 0.6604067f},
        {966, 0.6604067f},
        {967, 0.6604067f},
        {968, 0.6604067f},
        {969, 0.6604067f},
        {970, 0.6604067f},
        {971, 0.6604067f},
        {972, 0.6604067f},
        {973, 0.6604067f},
        {974, 0.6604067f},
        {975, 0.6604067f},
        {976, 0.6604067f},
        {977, 0.6604067f},
        {978, 0.6604067f},
        {979, 0.6604067f},
        {980, 0.6604067f},
        {981, 0.6604067f},
        {982, 0.6604067f},
        {983, 0.6604067f},
        {984, 0.6604067f},
        {985, 0.6604067f},
        {986, 0.6604067f},
        {987, 0.6604067f},
        {988, 0.6604067f},
        {989, 0.6604067f},
        {990, 0.6604067f},
        {991, 0.6604067f},
        {992, 0.6604067f},
        {993, 0.6604067f},
        {994, 0.6604067f},
        {995, 0.6604067f},
        {996, 0.6604067f},
        {997, 0.6604067f},
        {998, 0.6604067f},
        {999, 0.6604067f},
        {1000, 0.6604067f}
    };
}