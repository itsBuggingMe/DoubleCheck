using Microsoft.Extensions.Configuration;

namespace DoubleCheck
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            string outputPath = config.GetSection("outputPath").Value;

            try
            {
                Run(outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed with {ex.Message}\n{ex}");
            }
            finally
            {
                Console.WriteLine("Done");
                Console.ReadLine();
            }
        }

        static volatile int counter;
        static readonly object lockObj = new();

        private static void Run(string outputPath)
        {
            var bArr = Helper.AsBCharArr(KRYPTOS.C_K1);
            var logger = new FileLogger(outputPath);

            Parallel.For(0, Scrambler.AllWords.Length, i =>
            {
                OneLayerScramble<KRYPTOS, VigenereCipher> t = new(
                    checker: KRYPTOS.Instance,
                    cipher: new VigenereCipher(Scrambler.AllWordsString[i]),
                    cypherText: bArr,
                    saver: logger,
                    title: "K4"
                    );

                t.TryAllCombos();

                int ctrCopy;
                lock (lockObj)
                    ctrCopy = ++counter;

                if (counter % 4096 == 0)
                    Console.WriteLine("Done: " + counter);
            });
        }

        internal class KRYPTOS : IChecker
        {
            public static KRYPTOS Instance { get; } = new();

            public int GetIndex() => C_K4.IndexOf("NYPVTT");

            private readonly FChar[] fChars = "BERLIN".AsBCharArr();
            public ReadOnlySpan<FChar> GetMatch() => fChars;

            #region CypherText
            public const string KEY = "KRYPTOS";
            public const string K1_KEY = "PALIMPSEST";
            public const string K2_KEY = "ABSCISSA";

            public const string D_K1 = "EMUFPHZLRFAXYUSDJKZLDKRNSHGNFIVJ YQTQUXQBQVYUVLLTREVJYQTMKYRDMFD";
            public const string D_K2 = "VFPJUDEEHZWETZYVGWHKKQETGFQJNCE GGWHKK?DQMCPFQZDQMMIAGPFXHQRLG TIMVMZJANQLVKQEDAGDVFRPJUNGEUNA QZGZLECGYUXUEENJTBJLBQCRTBJDFHRR YIZETKZEMVDUFKSJHKFWHKUWQLSZFTI HHDDDUVH?DWKBFUFPWNTDFIYCUQZERE EVLDKFEZMOQQJLTTUGSYQPFEUNLAVIDX FLGGTEZ?FKZBSFDQVGOGIPUFXHHDRKF FHQNTGPUAECNUVPDJMQCLQUMUNEDFQ ELZZVRRGKFFVOEEXBDMVPNFQXEZLGRE DNQFMPNZGLFLPMRJQYALMGNUVPDXVKP DQUMEBEDMHDAFMJGZNUPLGEWJLLAETG";
            public const string D_K3 = "ENDYAHROHNLSRHEOCPTEOIBIDYSHNAIA CHTNREYULDSLLSLLNOHSNOSMRWXMNE TPRNGATIHNRARPESLNNELEBLPIIACAE WMTWNDITEENRAHCTENEUDRETNHAEOE TFOLSEDTIWENHAEIOYTEYQHEENCTAYCR EIFTBRSPAMHHEWENATAMATEGYEERLB TEEFOASFIOTUETUAEOTOARMAEERTNRTI BSEDDNIAAHTTMSTEWPIEROAGRIEWFEB AECTDDHILCEIHSITEGOEAOSDDRYDLORIT RKLMLEHAGTDHARDPNEOHMGFMFEUHE ECDMRIPFEIMEHNLSSTTRTVDOHW?";
            public const string D_K4 = "OBKR UOXOGHULBSOLIFBBWFLRVQQPRNGKSSO TWTQSJQSSEKZZWATJKLUDIAWINFBNYP VTTMZFPKWGDKZXTJCDIGKUHUAUEKCAR";

            public static readonly string C_K1 = string.Join(string.Empty, D_K1.Split(' ', '?'));
            public static readonly string C_K2 = string.Join(string.Empty, D_K2.Split(' ', '?'));
            public static readonly string C_K3 = string.Join(string.Empty, D_K3.Split(' ', '?'));
            public static readonly string C_K4 = string.Join(string.Empty, D_K4.Split(' ', '?'));
            #endregion;
        }
    }
}
