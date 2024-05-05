
using System.Text;

namespace DoubleCheck
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            string cyText = "OBKRUOXOGHULBSOLIFBBWFLRVQQPRNGKSSOTWTQSJQSSEKZZWATJKLUDIAWINFBNYPVTTMZFPKWGDKZXTJCDIGKUHUAUEKCAR";
            cyText = "ENDyaHrOHNLSRHEOCPTEOIBIDYSHNAIACHTNREYULDSLLSLLNOHSNOSMRWXMNETPRNGATIHNRARPESLNNELEBLPIIACAEWMTWNDITEENRAHCTENEUDRETNHAEOETFOLSEDTIWENHAEIOYTEYQHEENCTAYCREIFTBRSPAMHHEWENATAMATEGYEERLBTEEFOASFIOTUETUAEOTOARMAEERTNRTIBSEDDNIAAHTTMSTEWPIEROAGRIEWFEBAECTDDHILCEIHSITEGOEAOSDDRYDLORITRKLMLEHAGTDHARDPNEOHMGFMFEUHEECDMRIPFEIMEHNLSSTTRTVDOHW".ToUpper();
            OneLayerScramble<Transposition, KRYPTOS> t = new(
                cipher: new Transposition(cyText.Length, -2, 2), 
                cypherText: KRYPTOS.AsArray(cyText) //KRYPTOS.GetCleanCT(2, 4)
                );


            t.TryAllCombos();
        }

        internal class KRYPTOS : IChecker
        {
            private static readonly StringBuilder shared = new StringBuilder();

            private static int Sum(Span<BChar> bChars)
            {
                int s = 0;
                for (int i = 0; i < bChars.Length; i++)
                    s += bChars[i];
                return s;
            }

            private static readonly int BerlinSum = Sum(AsArray("BERLIN"));

            public static bool Check(Span<BChar> bChars)
            {
                for(int i = 0; i < bChars.Length - 6; i++)
                {
                    if(bChars.Slice(i).StartsWith("SLOWLY"))
                    {
                        return true;
                    }
                }

                return false;
            }

            public static BChar[] AsArray(string line)
            {
                BChar[] arr = new BChar[line.Length];
                for(int i = 0; i < arr.Length; i++)
                {
                    arr[i] = line[i];
                }
                return arr;
            }

            public static BChar[] GetCleanCT(int startLine, int endLine)
            {
                string[] lines = CypherText.Split("\r\n");

                if(startLine < 0 || endLine >= lines.Length)
                    throw new ArgumentException("nonono");

                BChar[] finalText = new BChar[lines[startLine..endLine].Sum(l => l.Length)];
                int counter = 0;
                for(int i = startLine; i < endLine; i++)
                {
                    foreach (char thing in lines[i])
                    {
                        finalText[counter++] = thing;
                    }
                }

                return finalText;
            }

            public static readonly string CypherText = 
                "EMUFPHZLRFAXYUSDJKZLDKRNSHGNFIVJ\r\n" +
                "YQTQUXQBQVYUVLLTREVJYQTMKYRDMFD\r\n" +
                "VFPJUDEEHZWETZYVGWHKKQETGFQJNCE\r\n" +
                "GGWHKKDQMCPFQZDQMMIAGPFXHQRLG\r\n" +//GGWHKK?DQMCPFQZDQMMIAGPFXHQRLG
                "TIMVMZJANQLVKQEDAGDVFRPJUNGEUNA\r\n" +
                "QZGZLECGYUXUEENJTBJLBQCRTBJDFHRR\r\n" +
                "YIZETKZEMVDUFKSJHKFWHKUWQLSZFTI\r\n" +
                "HHDDDUVH?DWKBFUFPWNTDFIYCUQZERE\r\n" +
                "EVLDKFEZMOQQJLTTUGSYQPFEUNLAVIDX\r\n" +
                "FLGGTEZ?FKZBSFDQVGOGIPUFXHHDRKF\r\n" +
                "FHQNTGPUAECNUVPDJMQCLQUMUNEDFQ\r\n" +
                "ELZZVRRGKFFVOEEXBDMVPNFQXEZLGRE\r\n" +
                "DNQFMPNZGLFLPMRJQYALMGNUVPDXVKP\r\n" +
                "DQUMEBEDMHDAFMJGZNUPLGEWJLLAETG\r\n" +
                "ENDYAHROHNLSRHEOCPTEOIBIDYSHNAIA\r\n" +
                "CHTNREYULDSLLSLLNOHSNOSMRWXMNE\r\n" +
                "TPRNGATIHNRARPESLNNELEBLPIIACAE\r\n" +
                "WMTWNDITEENRAHCTENEUDRETNHAEOE\r\n" +
                "TFOLSEDTIWENHAEIOYTEYQHEENCTAYCR\r\n" +
                "EIFTBRSPAMHHEWENATAMATEGYEERLB\r\n" +
                "TEEFOASFIOTUETUAEOTOARMAEERTNRTI\r\n" +
                "BSEDDNIAAHTTMSTEWPIEROAGRIEWFEB\r\n" +
                "AECTDDHILCEIHSITEGOEAOSDDRYDLORIT\r\n" +
                "RKLMLEHAGTDHARDPNEOHMGFMFEUHE\r\n" +
                "ECDMRIPFEIMEHNLSSTTRTVDOHWOBKR\r\n" +//ECDMRIPFEIMEHNLSSTTRTVDOHW?OBKR
                "UOXOGHULBSOLIFBBWFLRVQQPRNGKSSO\r\n" +
                "TWTQSJQSSEKZZWATJKLUDIAWINFBNYP\r\n" +
                "VTTMZFPKWGDKZXTJCDIGKUHUAUEKCAR";
        }

        public static bool StartsWith(this Span<BChar> bString, string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != bString[i])
                    return false;
            }

            return true;
        }

        public static string ArrToString(this BChar[] arr)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach(var r in arr)
            {
                stringBuilder.Append((char)r);
            }
            return stringBuilder.ToString();
        }
    }
}
