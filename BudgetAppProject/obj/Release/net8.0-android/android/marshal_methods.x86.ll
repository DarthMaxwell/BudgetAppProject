; ModuleID = 'marshal_methods.x86.ll'
source_filename = "marshal_methods.x86.ll"
target datalayout = "e-m:e-p:32:32-p270:32:32-p271:32:32-p272:64:64-f64:32:64-f80:32-n8:16:32-S128"
target triple = "i686-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [118 x ptr] zeroinitializer, align 4

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [236 x i32] [
	i32 38948123, ; 0: ar\Microsoft.Maui.Controls.resources.dll => 0x2524d1b => 0
	i32 42244203, ; 1: he\Microsoft.Maui.Controls.resources.dll => 0x284986b => 9
	i32 42639949, ; 2: System.Threading.Thread => 0x28aa24d => 109
	i32 67008169, ; 3: zh-Hant\Microsoft.Maui.Controls.resources => 0x3fe76a9 => 33
	i32 72070932, ; 4: Microsoft.Maui.Graphics.dll => 0x44bb714 => 47
	i32 83839681, ; 5: ms\Microsoft.Maui.Controls.resources.dll => 0x4ff4ac1 => 17
	i32 117431740, ; 6: System.Runtime.InteropServices => 0x6ffddbc => 104
	i32 136584136, ; 7: zh-Hans\Microsoft.Maui.Controls.resources.dll => 0x8241bc8 => 32
	i32 140062828, ; 8: sk\Microsoft.Maui.Controls.resources.dll => 0x859306c => 25
	i32 165246403, ; 9: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 57
	i32 182336117, ; 10: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 75
	i32 205061960, ; 11: System.ComponentModel => 0xc38ff48 => 88
	i32 317674968, ; 12: vi\Microsoft.Maui.Controls.resources => 0x12ef55d8 => 30
	i32 318968648, ; 13: Xamarin.AndroidX.Activity.dll => 0x13031348 => 53
	i32 321963286, ; 14: fr\Microsoft.Maui.Controls.resources.dll => 0x1330c516 => 8
	i32 342366114, ; 15: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 64
	i32 347068432, ; 16: SQLitePCLRaw.lib.e_sqlite3.android.dll => 0x14afd810 => 51
	i32 379916513, ; 17: System.Threading.Thread.dll => 0x16a510e1 => 109
	i32 385762202, ; 18: System.Memory.dll => 0x16fe439a => 95
	i32 395744057, ; 19: _Microsoft.Android.Resource.Designer => 0x17969339 => 34
	i32 409257351, ; 20: tr\Microsoft.Maui.Controls.resources.dll => 0x1864c587 => 28
	i32 442565967, ; 21: System.Collections => 0x1a61054f => 85
	i32 450948140, ; 22: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 63
	i32 469710990, ; 23: System.dll => 0x1bff388e => 112
	i32 489220957, ; 24: es\Microsoft.Maui.Controls.resources.dll => 0x1d28eb5d => 6
	i32 498788369, ; 25: System.ObjectModel => 0x1dbae811 => 100
	i32 513247710, ; 26: Microsoft.Extensions.Primitives.dll => 0x1e9789de => 42
	i32 538707440, ; 27: th\Microsoft.Maui.Controls.resources.dll => 0x201c05f0 => 27
	i32 539058512, ; 28: Microsoft.Extensions.Logging => 0x20216150 => 39
	i32 627609679, ; 29: Xamarin.AndroidX.CustomView => 0x2568904f => 61
	i32 627931235, ; 30: nl\Microsoft.Maui.Controls.resources => 0x256d7863 => 19
	i32 672442732, ; 31: System.Collections.Concurrent => 0x2814a96c => 83
	i32 722857257, ; 32: System.Runtime.Loader.dll => 0x2b15ed29 => 105
	i32 734383066, ; 33: BudgetAppProject.dll => 0x2bc5cbda => 82
	i32 748832960, ; 34: SQLitePCLRaw.batteries_v2 => 0x2ca248c0 => 49
	i32 759454413, ; 35: System.Net.Requests => 0x2d445acd => 98
	i32 775507847, ; 36: System.IO.Compression => 0x2e394f87 => 92
	i32 777317022, ; 37: sk\Microsoft.Maui.Controls.resources => 0x2e54ea9e => 25
	i32 789151979, ; 38: Microsoft.Extensions.Options => 0x2f0980eb => 41
	i32 823281589, ; 39: System.Private.Uri.dll => 0x311247b5 => 101
	i32 830298997, ; 40: System.IO.Compression.Brotli => 0x317d5b75 => 91
	i32 869139383, ; 41: hi\Microsoft.Maui.Controls.resources.dll => 0x33ce03b7 => 10
	i32 880668424, ; 42: ru\Microsoft.Maui.Controls.resources.dll => 0x347def08 => 24
	i32 887001765, ; 43: BudgetAppProject => 0x34de92a5 => 82
	i32 904024072, ; 44: System.ComponentModel.Primitives.dll => 0x35e25008 => 86
	i32 918734561, ; 45: pt-BR\Microsoft.Maui.Controls.resources.dll => 0x36c2c6e1 => 21
	i32 961460050, ; 46: it\Microsoft.Maui.Controls.resources.dll => 0x394eb752 => 14
	i32 967690846, ; 47: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 64
	i32 990727110, ; 48: ro\Microsoft.Maui.Controls.resources.dll => 0x3b0d4bc6 => 23
	i32 992768348, ; 49: System.Collections.dll => 0x3b2c715c => 85
	i32 1012816738, ; 50: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 74
	i32 1028951442, ; 51: Microsoft.Extensions.DependencyInjection.Abstractions => 0x3d548d92 => 38
	i32 1035644815, ; 52: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 54
	i32 1043950537, ; 53: de\Microsoft.Maui.Controls.resources.dll => 0x3e396bc9 => 4
	i32 1044663988, ; 54: System.Linq.Expressions.dll => 0x3e444eb4 => 93
	i32 1052210849, ; 55: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 66
	i32 1082857460, ; 56: System.ComponentModel.TypeConverter => 0x408b17f4 => 87
	i32 1084122840, ; 57: Xamarin.Kotlin.StdLib => 0x409e66d8 => 79
	i32 1098259244, ; 58: System => 0x41761b2c => 112
	i32 1108272742, ; 59: sv\Microsoft.Maui.Controls.resources.dll => 0x420ee666 => 26
	i32 1117529484, ; 60: pl\Microsoft.Maui.Controls.resources.dll => 0x429c258c => 20
	i32 1118262833, ; 61: ko\Microsoft.Maui.Controls.resources => 0x42a75631 => 16
	i32 1154501435, ; 62: BudgetAppLibray => 0x44d04b3b => 81
	i32 1168523401, ; 63: pt\Microsoft.Maui.Controls.resources => 0x45a64089 => 22
	i32 1178241025, ; 64: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 71
	i32 1260983243, ; 65: cs\Microsoft.Maui.Controls.resources => 0x4b2913cb => 2
	i32 1292207520, ; 66: SQLitePCLRaw.core.dll => 0x4d0585a0 => 50
	i32 1293217323, ; 67: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 62
	i32 1308624726, ; 68: hr\Microsoft.Maui.Controls.resources.dll => 0x4e000756 => 11
	i32 1324164729, ; 69: System.Linq => 0x4eed2679 => 94
	i32 1336711579, ; 70: zh-HK\Microsoft.Maui.Controls.resources.dll => 0x4fac999b => 31
	i32 1373134921, ; 71: zh-Hans\Microsoft.Maui.Controls.resources => 0x51d86049 => 32
	i32 1376866003, ; 72: Xamarin.AndroidX.SavedState => 0x52114ed3 => 74
	i32 1406073936, ; 73: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 58
	i32 1430672901, ; 74: ar\Microsoft.Maui.Controls.resources => 0x55465605 => 0
	i32 1461004990, ; 75: es\Microsoft.Maui.Controls.resources => 0x57152abe => 6
	i32 1462112819, ; 76: System.IO.Compression.dll => 0x57261233 => 92
	i32 1469204771, ; 77: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 55
	i32 1470490898, ; 78: Microsoft.Extensions.Primitives => 0x57a5e912 => 42
	i32 1480492111, ; 79: System.IO.Compression.Brotli.dll => 0x583e844f => 91
	i32 1526286932, ; 80: vi\Microsoft.Maui.Controls.resources.dll => 0x5af94a54 => 30
	i32 1543031311, ; 81: System.Text.RegularExpressions.dll => 0x5bf8ca0f => 108
	i32 1622152042, ; 82: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 68
	i32 1624863272, ; 83: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 77
	i32 1636350590, ; 84: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 60
	i32 1639515021, ; 85: System.Net.Http.dll => 0x61b9038d => 96
	i32 1639986890, ; 86: System.Text.RegularExpressions => 0x61c036ca => 108
	i32 1657153582, ; 87: System.Runtime => 0x62c6282e => 106
	i32 1658251792, ; 88: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 78
	i32 1677501392, ; 89: System.Net.Primitives.dll => 0x63fca3d0 => 97
	i32 1679769178, ; 90: System.Security.Cryptography => 0x641f3e5a => 107
	i32 1711441057, ; 91: SQLitePCLRaw.lib.e_sqlite3.android => 0x660284a1 => 51
	i32 1729485958, ; 92: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 56
	i32 1743415430, ; 93: ca\Microsoft.Maui.Controls.resources => 0x67ea6886 => 1
	i32 1766324549, ; 94: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 75
	i32 1770582343, ; 95: Microsoft.Extensions.Logging.dll => 0x6988f147 => 39
	i32 1780572499, ; 96: Mono.Android.Runtime.dll => 0x6a216153 => 116
	i32 1782862114, ; 97: ms\Microsoft.Maui.Controls.resources => 0x6a445122 => 17
	i32 1788241197, ; 98: Xamarin.AndroidX.Fragment => 0x6a96652d => 63
	i32 1793755602, ; 99: he\Microsoft.Maui.Controls.resources => 0x6aea89d2 => 9
	i32 1808609942, ; 100: Xamarin.AndroidX.Loader => 0x6bcd3296 => 68
	i32 1813058853, ; 101: Xamarin.Kotlin.StdLib.dll => 0x6c111525 => 79
	i32 1813201214, ; 102: Xamarin.Google.Android.Material => 0x6c13413e => 78
	i32 1818569960, ; 103: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 72
	i32 1828688058, ; 104: Microsoft.Extensions.Logging.Abstractions.dll => 0x6cff90ba => 40
	i32 1853025655, ; 105: sv\Microsoft.Maui.Controls.resources => 0x6e72ed77 => 26
	i32 1858542181, ; 106: System.Linq.Expressions => 0x6ec71a65 => 93
	i32 1875935024, ; 107: fr\Microsoft.Maui.Controls.resources => 0x6fd07f30 => 8
	i32 1893218855, ; 108: cs\Microsoft.Maui.Controls.resources.dll => 0x70d83a27 => 2
	i32 1910275211, ; 109: System.Collections.NonGeneric.dll => 0x71dc7c8b => 84
	i32 1953182387, ; 110: id\Microsoft.Maui.Controls.resources.dll => 0x746b32b3 => 13
	i32 1968388702, ; 111: Microsoft.Extensions.Configuration.dll => 0x75533a5e => 35
	i32 2003115576, ; 112: el\Microsoft.Maui.Controls.resources => 0x77651e38 => 5
	i32 2019465201, ; 113: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 66
	i32 2045470958, ; 114: System.Private.Xml => 0x79eb68ee => 102
	i32 2055257422, ; 115: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 65
	i32 2066184531, ; 116: de\Microsoft.Maui.Controls.resources => 0x7b277953 => 4
	i32 2079903147, ; 117: System.Runtime.dll => 0x7bf8cdab => 106
	i32 2090596640, ; 118: System.Numerics.Vectors => 0x7c9bf920 => 99
	i32 2103459038, ; 119: SQLitePCLRaw.provider.e_sqlite3.dll => 0x7d603cde => 52
	i32 2127167465, ; 120: System.Console => 0x7ec9ffe9 => 89
	i32 2159891885, ; 121: Microsoft.Maui => 0x80bd55ad => 45
	i32 2169148018, ; 122: hu\Microsoft.Maui.Controls.resources => 0x814a9272 => 12
	i32 2181898931, ; 123: Microsoft.Extensions.Options.dll => 0x820d22b3 => 41
	i32 2192057212, ; 124: Microsoft.Extensions.Logging.Abstractions => 0x82a8237c => 40
	i32 2193016926, ; 125: System.ObjectModel.dll => 0x82b6c85e => 100
	i32 2201107256, ; 126: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x83323b38 => 80
	i32 2201231467, ; 127: System.Net.Http => 0x8334206b => 96
	i32 2207618523, ; 128: it\Microsoft.Maui.Controls.resources => 0x839595db => 14
	i32 2266799131, ; 129: Microsoft.Extensions.Configuration.Abstractions => 0x871c9c1b => 36
	i32 2279755925, ; 130: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 73
	i32 2303942373, ; 131: nb\Microsoft.Maui.Controls.resources => 0x89535ee5 => 18
	i32 2305521784, ; 132: System.Private.CoreLib.dll => 0x896b7878 => 114
	i32 2340441535, ; 133: System.Runtime.InteropServices.RuntimeInformation.dll => 0x8b804dbf => 103
	i32 2353062107, ; 134: System.Net.Primitives => 0x8c40e0db => 97
	i32 2366048013, ; 135: hu\Microsoft.Maui.Controls.resources.dll => 0x8d07070d => 12
	i32 2368005991, ; 136: System.Xml.ReaderWriter.dll => 0x8d24e767 => 111
	i32 2371007202, ; 137: Microsoft.Extensions.Configuration => 0x8d52b2e2 => 35
	i32 2395872292, ; 138: id\Microsoft.Maui.Controls.resources => 0x8ece1c24 => 13
	i32 2427813419, ; 139: hi\Microsoft.Maui.Controls.resources => 0x90b57e2b => 10
	i32 2435356389, ; 140: System.Console.dll => 0x912896e5 => 89
	i32 2465273461, ; 141: SQLitePCLRaw.batteries_v2.dll => 0x92f11675 => 49
	i32 2471841756, ; 142: netstandard.dll => 0x93554fdc => 113
	i32 2475788418, ; 143: Java.Interop.dll => 0x93918882 => 115
	i32 2480646305, ; 144: Microsoft.Maui.Controls => 0x93dba8a1 => 43
	i32 2503351294, ; 145: ko\Microsoft.Maui.Controls.resources.dll => 0x95361bfe => 16
	i32 2550873716, ; 146: hr\Microsoft.Maui.Controls.resources => 0x980b3e74 => 11
	i32 2576534780, ; 147: ja\Microsoft.Maui.Controls.resources.dll => 0x9992ccfc => 15
	i32 2593496499, ; 148: pl\Microsoft.Maui.Controls.resources => 0x9a959db3 => 20
	i32 2605712449, ; 149: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x9b500441 => 80
	i32 2617129537, ; 150: System.Private.Xml.dll => 0x9bfe3a41 => 102
	i32 2620871830, ; 151: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 60
	i32 2626831493, ; 152: ja\Microsoft.Maui.Controls.resources => 0x9c924485 => 15
	i32 2663698177, ; 153: System.Runtime.Loader => 0x9ec4cf01 => 105
	i32 2732626843, ; 154: Xamarin.AndroidX.Activity => 0xa2e0939b => 53
	i32 2737747696, ; 155: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 55
	i32 2740698338, ; 156: ca\Microsoft.Maui.Controls.resources.dll => 0xa35bbce2 => 1
	i32 2752995522, ; 157: pt-BR\Microsoft.Maui.Controls.resources => 0xa41760c2 => 21
	i32 2758225723, ; 158: Microsoft.Maui.Controls.Xaml => 0xa4672f3b => 44
	i32 2764765095, ; 159: Microsoft.Maui.dll => 0xa4caf7a7 => 45
	i32 2778768386, ; 160: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 76
	i32 2785988530, ; 161: th\Microsoft.Maui.Controls.resources => 0xa60ecfb2 => 27
	i32 2801831435, ; 162: Microsoft.Maui.Graphics => 0xa7008e0b => 47
	i32 2810250172, ; 163: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 58
	i32 2853208004, ; 164: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 76
	i32 2861189240, ; 165: Microsoft.Maui.Essentials => 0xaa8a4878 => 46
	i32 2909740682, ; 166: System.Private.CoreLib => 0xad6f1e8a => 114
	i32 2916838712, ; 167: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 77
	i32 2919462931, ; 168: System.Numerics.Vectors.dll => 0xae037813 => 99
	i32 2959614098, ; 169: System.ComponentModel.dll => 0xb0682092 => 88
	i32 2978675010, ; 170: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 62
	i32 3038032645, ; 171: _Microsoft.Android.Resource.Designer.dll => 0xb514b305 => 34
	i32 3053864966, ; 172: fi\Microsoft.Maui.Controls.resources.dll => 0xb6064806 => 7
	i32 3057625584, ; 173: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 69
	i32 3059408633, ; 174: Mono.Android.Runtime => 0xb65adef9 => 116
	i32 3059793426, ; 175: System.ComponentModel.Primitives => 0xb660be12 => 86
	i32 3161931734, ; 176: BudgetAppLibray.dll => 0xbc773fd6 => 81
	i32 3178803400, ; 177: Xamarin.AndroidX.Navigation.Fragment.dll => 0xbd78b0c8 => 70
	i32 3220365878, ; 178: System.Threading => 0xbff2e236 => 110
	i32 3258312781, ; 179: Xamarin.AndroidX.CardView => 0xc235e84d => 56
	i32 3286872994, ; 180: SQLite-net.dll => 0xc3e9b3a2 => 48
	i32 3305363605, ; 181: fi\Microsoft.Maui.Controls.resources => 0xc503d895 => 7
	i32 3316684772, ; 182: System.Net.Requests.dll => 0xc5b097e4 => 98
	i32 3317135071, ; 183: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 61
	i32 3346324047, ; 184: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 71
	i32 3357674450, ; 185: ru\Microsoft.Maui.Controls.resources => 0xc8220bd2 => 24
	i32 3360279109, ; 186: SQLitePCLRaw.core => 0xc849ca45 => 50
	i32 3362522851, ; 187: Xamarin.AndroidX.Core => 0xc86c06e3 => 59
	i32 3366347497, ; 188: Java.Interop => 0xc8a662e9 => 115
	i32 3374999561, ; 189: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 73
	i32 3381016424, ; 190: da\Microsoft.Maui.Controls.resources => 0xc9863768 => 3
	i32 3428513518, ; 191: Microsoft.Extensions.DependencyInjection.dll => 0xcc5af6ee => 37
	i32 3430777524, ; 192: netstandard => 0xcc7d82b4 => 113
	i32 3458724246, ; 193: pt\Microsoft.Maui.Controls.resources.dll => 0xce27f196 => 22
	i32 3471940407, ; 194: System.ComponentModel.TypeConverter.dll => 0xcef19b37 => 87
	i32 3476120550, ; 195: Mono.Android => 0xcf3163e6 => 117
	i32 3484440000, ; 196: ro\Microsoft.Maui.Controls.resources => 0xcfb055c0 => 23
	i32 3580758918, ; 197: zh-HK\Microsoft.Maui.Controls.resources => 0xd56e0b86 => 31
	i32 3592228221, ; 198: zh-Hant\Microsoft.Maui.Controls.resources.dll => 0xd61d0d7d => 33
	i32 3608519521, ; 199: System.Linq.dll => 0xd715a361 => 94
	i32 3624195450, ; 200: System.Runtime.InteropServices.RuntimeInformation => 0xd804d57a => 103
	i32 3641597786, ; 201: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 65
	i32 3643446276, ; 202: tr\Microsoft.Maui.Controls.resources => 0xd92a9404 => 28
	i32 3643854240, ; 203: Xamarin.AndroidX.Navigation.Fragment => 0xd930cda0 => 70
	i32 3657292374, ; 204: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd9fdda56 => 36
	i32 3672681054, ; 205: Mono.Android.dll => 0xdae8aa5e => 117
	i32 3724971120, ; 206: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 69
	i32 3748608112, ; 207: System.Diagnostics.DiagnosticSource => 0xdf6f3870 => 90
	i32 3751619990, ; 208: da\Microsoft.Maui.Controls.resources.dll => 0xdf9d2d96 => 3
	i32 3754567612, ; 209: SQLitePCLRaw.provider.e_sqlite3 => 0xdfca27bc => 52
	i32 3786282454, ; 210: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 57
	i32 3792276235, ; 211: System.Collections.NonGeneric => 0xe2098b0b => 84
	i32 3823082795, ; 212: System.Security.Cryptography.dll => 0xe3df9d2b => 107
	i32 3841636137, ; 213: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xe4fab729 => 38
	i32 3849253459, ; 214: System.Runtime.InteropServices.dll => 0xe56ef253 => 104
	i32 3876362041, ; 215: SQLite-net => 0xe70c9739 => 48
	i32 3896106733, ; 216: System.Collections.Concurrent.dll => 0xe839deed => 83
	i32 3896760992, ; 217: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 59
	i32 3920221145, ; 218: nl\Microsoft.Maui.Controls.resources.dll => 0xe9a9d3d9 => 19
	i32 3928044579, ; 219: System.Xml.ReaderWriter => 0xea213423 => 111
	i32 3931092270, ; 220: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 72
	i32 3955647286, ; 221: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 54
	i32 4025784931, ; 222: System.Memory => 0xeff49a63 => 95
	i32 4046471985, ; 223: Microsoft.Maui.Controls.Xaml.dll => 0xf1304331 => 44
	i32 4073602200, ; 224: System.Threading.dll => 0xf2ce3c98 => 110
	i32 4091086043, ; 225: el\Microsoft.Maui.Controls.resources.dll => 0xf3d904db => 5
	i32 4094352644, ; 226: Microsoft.Maui.Essentials.dll => 0xf40add04 => 46
	i32 4100113165, ; 227: System.Private.Uri => 0xf462c30d => 101
	i32 4103439459, ; 228: uk\Microsoft.Maui.Controls.resources.dll => 0xf4958463 => 29
	i32 4126470640, ; 229: Microsoft.Extensions.DependencyInjection => 0xf5f4f1f0 => 37
	i32 4150914736, ; 230: uk\Microsoft.Maui.Controls.resources => 0xf769eeb0 => 29
	i32 4182413190, ; 231: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 67
	i32 4213026141, ; 232: System.Diagnostics.DiagnosticSource.dll => 0xfb1dad5d => 90
	i32 4249188766, ; 233: nb\Microsoft.Maui.Controls.resources.dll => 0xfd45799e => 18
	i32 4271975918, ; 234: Microsoft.Maui.Controls.dll => 0xfea12dee => 43
	i32 4292120959 ; 235: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 67
], align 4

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [236 x i32] [
	i32 0, ; 0
	i32 9, ; 1
	i32 109, ; 2
	i32 33, ; 3
	i32 47, ; 4
	i32 17, ; 5
	i32 104, ; 6
	i32 32, ; 7
	i32 25, ; 8
	i32 57, ; 9
	i32 75, ; 10
	i32 88, ; 11
	i32 30, ; 12
	i32 53, ; 13
	i32 8, ; 14
	i32 64, ; 15
	i32 51, ; 16
	i32 109, ; 17
	i32 95, ; 18
	i32 34, ; 19
	i32 28, ; 20
	i32 85, ; 21
	i32 63, ; 22
	i32 112, ; 23
	i32 6, ; 24
	i32 100, ; 25
	i32 42, ; 26
	i32 27, ; 27
	i32 39, ; 28
	i32 61, ; 29
	i32 19, ; 30
	i32 83, ; 31
	i32 105, ; 32
	i32 82, ; 33
	i32 49, ; 34
	i32 98, ; 35
	i32 92, ; 36
	i32 25, ; 37
	i32 41, ; 38
	i32 101, ; 39
	i32 91, ; 40
	i32 10, ; 41
	i32 24, ; 42
	i32 82, ; 43
	i32 86, ; 44
	i32 21, ; 45
	i32 14, ; 46
	i32 64, ; 47
	i32 23, ; 48
	i32 85, ; 49
	i32 74, ; 50
	i32 38, ; 51
	i32 54, ; 52
	i32 4, ; 53
	i32 93, ; 54
	i32 66, ; 55
	i32 87, ; 56
	i32 79, ; 57
	i32 112, ; 58
	i32 26, ; 59
	i32 20, ; 60
	i32 16, ; 61
	i32 81, ; 62
	i32 22, ; 63
	i32 71, ; 64
	i32 2, ; 65
	i32 50, ; 66
	i32 62, ; 67
	i32 11, ; 68
	i32 94, ; 69
	i32 31, ; 70
	i32 32, ; 71
	i32 74, ; 72
	i32 58, ; 73
	i32 0, ; 74
	i32 6, ; 75
	i32 92, ; 76
	i32 55, ; 77
	i32 42, ; 78
	i32 91, ; 79
	i32 30, ; 80
	i32 108, ; 81
	i32 68, ; 82
	i32 77, ; 83
	i32 60, ; 84
	i32 96, ; 85
	i32 108, ; 86
	i32 106, ; 87
	i32 78, ; 88
	i32 97, ; 89
	i32 107, ; 90
	i32 51, ; 91
	i32 56, ; 92
	i32 1, ; 93
	i32 75, ; 94
	i32 39, ; 95
	i32 116, ; 96
	i32 17, ; 97
	i32 63, ; 98
	i32 9, ; 99
	i32 68, ; 100
	i32 79, ; 101
	i32 78, ; 102
	i32 72, ; 103
	i32 40, ; 104
	i32 26, ; 105
	i32 93, ; 106
	i32 8, ; 107
	i32 2, ; 108
	i32 84, ; 109
	i32 13, ; 110
	i32 35, ; 111
	i32 5, ; 112
	i32 66, ; 113
	i32 102, ; 114
	i32 65, ; 115
	i32 4, ; 116
	i32 106, ; 117
	i32 99, ; 118
	i32 52, ; 119
	i32 89, ; 120
	i32 45, ; 121
	i32 12, ; 122
	i32 41, ; 123
	i32 40, ; 124
	i32 100, ; 125
	i32 80, ; 126
	i32 96, ; 127
	i32 14, ; 128
	i32 36, ; 129
	i32 73, ; 130
	i32 18, ; 131
	i32 114, ; 132
	i32 103, ; 133
	i32 97, ; 134
	i32 12, ; 135
	i32 111, ; 136
	i32 35, ; 137
	i32 13, ; 138
	i32 10, ; 139
	i32 89, ; 140
	i32 49, ; 141
	i32 113, ; 142
	i32 115, ; 143
	i32 43, ; 144
	i32 16, ; 145
	i32 11, ; 146
	i32 15, ; 147
	i32 20, ; 148
	i32 80, ; 149
	i32 102, ; 150
	i32 60, ; 151
	i32 15, ; 152
	i32 105, ; 153
	i32 53, ; 154
	i32 55, ; 155
	i32 1, ; 156
	i32 21, ; 157
	i32 44, ; 158
	i32 45, ; 159
	i32 76, ; 160
	i32 27, ; 161
	i32 47, ; 162
	i32 58, ; 163
	i32 76, ; 164
	i32 46, ; 165
	i32 114, ; 166
	i32 77, ; 167
	i32 99, ; 168
	i32 88, ; 169
	i32 62, ; 170
	i32 34, ; 171
	i32 7, ; 172
	i32 69, ; 173
	i32 116, ; 174
	i32 86, ; 175
	i32 81, ; 176
	i32 70, ; 177
	i32 110, ; 178
	i32 56, ; 179
	i32 48, ; 180
	i32 7, ; 181
	i32 98, ; 182
	i32 61, ; 183
	i32 71, ; 184
	i32 24, ; 185
	i32 50, ; 186
	i32 59, ; 187
	i32 115, ; 188
	i32 73, ; 189
	i32 3, ; 190
	i32 37, ; 191
	i32 113, ; 192
	i32 22, ; 193
	i32 87, ; 194
	i32 117, ; 195
	i32 23, ; 196
	i32 31, ; 197
	i32 33, ; 198
	i32 94, ; 199
	i32 103, ; 200
	i32 65, ; 201
	i32 28, ; 202
	i32 70, ; 203
	i32 36, ; 204
	i32 117, ; 205
	i32 69, ; 206
	i32 90, ; 207
	i32 3, ; 208
	i32 52, ; 209
	i32 57, ; 210
	i32 84, ; 211
	i32 107, ; 212
	i32 38, ; 213
	i32 104, ; 214
	i32 48, ; 215
	i32 83, ; 216
	i32 59, ; 217
	i32 19, ; 218
	i32 111, ; 219
	i32 72, ; 220
	i32 54, ; 221
	i32 95, ; 222
	i32 44, ; 223
	i32 110, ; 224
	i32 5, ; 225
	i32 46, ; 226
	i32 101, ; 227
	i32 29, ; 228
	i32 37, ; 229
	i32 29, ; 230
	i32 67, ; 231
	i32 90, ; 232
	i32 18, ; 233
	i32 43, ; 234
	i32 67 ; 235
], align 4

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 4

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 4

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 4

; Functions

; Function attributes: "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" uwtable willreturn
define void @xamarin_app_init(ptr nocapture noundef readnone %env, ptr noundef %fn) local_unnamed_addr #0
{
	%fnIsNull = icmp eq ptr %fn, null
	br i1 %fnIsNull, label %1, label %2

1: ; preds = %0
	%putsResult = call noundef i32 @puts(ptr @.str.0)
	call void @abort()
	unreachable 

2: ; preds = %1, %0
	store ptr %fn, ptr @get_function_pointer, align 4, !tbaa !3
	ret void
}

; Strings
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 1

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "stackrealign" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "stackrealign" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" }

; Metadata
!llvm.module.flags = !{!0, !1, !7}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!"Xamarin.Android remotes/origin/release/8.0.1xx @ af27162bee43b7fecdca59b4f67aa8c175cbc875"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"NumRegisterParameters", i32 0}
