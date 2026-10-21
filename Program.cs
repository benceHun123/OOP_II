using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_II
{
    public enum KolcsonzesEredmeny
    {
        Sikeres,
        MarKolcsonozve,
        NincsKolcsonozve
    }

    public class KolcsonozhetoEszkoz 
    {
        private bool kolcsonozve;
        public string Megnevezes { get; }
        private decimal napidij;
        public decimal NapiDij
        {
            get { return napidij; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(napidij), "A napidíjnak nagyobbnak kell lenni mint 0.");
                }
                napidij=value;
            }
        }
        public bool Kolcsonozve => kolcsonozve;

        public KolcsonozhetoEszkoz(string megnevezes, decimal napidij)
        {
            if (string.IsNullOrWhiteSpace(megnevezes))
            {
                throw new ArgumentException(nameof(megnevezes), "Nem lehet üres vagy csak szóköz.");
            }
            Megnevezes = megnevezes;
            NapiDij = napidij;

        }

        public KolcsonzesEredmeny Kolcsonoz()
        {
            if (kolcsonozve ==true)
            {
                return KolcsonzesEredmeny.MarKolcsonozve;
            }
            kolcsonozve = true;
            return KolcsonzesEredmeny.Sikeres;
        }
        public KolcsonzesEredmeny Visszavesz()
        {
            if (!kolcsonozve)
            {
                return KolcsonzesEredmeny.NincsKolcsonozve;
            }
            kolcsonozve = false;
            return KolcsonzesEredmeny.Sikeres;
        }

        public virtual decimal KolcsonzesiDij(int napok)
        {
            if (napok <=0)
            {
                throw new ArgumentOutOfRangeException(nameof(napok), "A napoknak pozitívnak kell lennie.");
            }
            return NapiDij * napok;
        }

        public override string ToString()
        {
            if (kolcsonozve == true)
            {
                return $"Megnevezés: {Megnevezes} | Napi díj: {NapiDij:N2} Ft | Állapot: kölcsönözve";
            }
            return $"Megnevezés: {Megnevezes} | Napi díj: {NapiDij} | Állapot: szabad";

        }

    }

    public class Kerekpar : KolcsonozhetoEszkoz
    {
        public int Vazmeret { get; }

        public Kerekpar(string megnevezes, decimal napidij, int vazmeret): base(megnevezes,napidij)
        {
            if (vazmeret<=0)
            {
                throw new ArgumentOutOfRangeException(nameof(vazmeret), "Pozitívnak kell lennie.");
            }
            Vazmeret = vazmeret;
        }

        public override string ToString()
        {
            return base.ToString() + $" | Vázméret: {Vazmeret} cm";
        }
    }

    public class Sifelszereles : KolcsonozhetoEszkoz
    {
        public string Meret { get; }
        public decimal ElokeszitesiDij { get; }

        public Sifelszereles(string megnevezes, decimal napidij, string meret, decimal elokeszitesidij) : base(megnevezes, napidij)
        {
            if (string.IsNullOrWhiteSpace(meret))
            {
                throw new ArgumentException(nameof(meret),"Ne legyen üres.");
            }
            if (elokeszitesidij<0)
            {
                throw new ArgumentOutOfRangeException(nameof(elokeszitesidij),"Nem lehet negatív.");
            }
            Meret = meret;
            ElokeszitesiDij = elokeszitesidij;
        }

        public override decimal KolcsonzesiDij(int napok)
        {
            return base.KolcsonzesiDij(napok) + ElokeszitesiDij;
        }
        public override string ToString()
        {
            return base.ToString() + $" | Méret: {Meret} | Előkészítési díj: {ElokeszitesiDij:N2} Ft";
        }
    }

    public class SupDeszka : KolcsonozhetoEszkoz
    {
        public int MaxTerheles { get; }

        public SupDeszka(string megnevezes, int napidij, int maxTerheles) : base(megnevezes, napidij)
        {
            if (maxTerheles <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxTerheles), "Nem lehet nulla vagy kisebb");
            }
            MaxTerheles = maxTerheles;
        }
        public override string ToString()
        {
            return base.ToString() + $" | Max terhelés: {MaxTerheles}";
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            List<KolcsonozhetoEszkoz> eszkozok = new List<KolcsonozhetoEszkoz>()
            {
                new KolcsonozhetoEszkoz("Gördeszka", 1500m),
                new Kerekpar("Városi kerékpár", 2500m, 50),
                new Kerekpar("Túrakerékpár", 3000m, 56),
                new Sifelszereles("Felnőtt sífelszerelés", 4000m, "L", 1500m),
                new Sifelszereles("Junior sífelszerelés", 3500m, "M", 1000m),
                new KolcsonozhetoEszkoz("Bukósisak", 1000m),
                new SupDeszka("Deszka",1000,200)
            };
            foreach (var eszkoz in eszkozok)
            {
                Console.WriteLine($"{eszkoz.ToString()} | Háromnapos kölcsönzési díj: {eszkoz.KolcsonzesiDij(3)} Ft");
            }
            Console.WriteLine();
            Console.WriteLine(EredmenyUzenet(eszkozok[1].Kolcsonoz()));
            Console.WriteLine(EredmenyUzenet(eszkozok[1].Kolcsonoz()));
            Console.WriteLine(EredmenyUzenet(eszkozok[3].Kolcsonoz()));
            Console.WriteLine(EredmenyUzenet(eszkozok[1].Visszavesz()));
            Console.WriteLine(EredmenyUzenet(eszkozok[1].Visszavesz()));

            try
            {
                eszkozok[0].NapiDij = 0;
            }
            catch(ArgumentOutOfRangeException hiba) 
            {
                Console.WriteLine($"\nHiba: {hiba.Message} | Díj: {eszkozok[0].NapiDij}");
            }

            try
            {
                eszkozok[3].KolcsonzesiDij(0);
            }
            catch (ArgumentOutOfRangeException hiba)
            {
                Console.WriteLine($"\nHiba: {hiba.Message}");
            }
            /*13.Feladat LINQ*/
            Console.WriteLine("\nSzabad eszközök megnevezés szerint növekvő sorrendben");
            foreach (var eszkoz in eszkozok.Where(e=>e.Kolcsonozve == false).OrderBy(x=>x.Megnevezes))
            {
                Console.WriteLine(eszkoz);
            }

            Console.WriteLine($"\nKölcsönzött eszközök száma: {eszkozok.Count(e=>e.Kolcsonozve == true)}");

            Console.WriteLine($"\nÖsszes háromnapos díj: {eszkozok.Sum(e=>e.KolcsonzesiDij(3))} ");

            var elem = eszkozok.OrderByDescending(e => e.NapiDij).First();
            Console.WriteLine($"\nLegmagasabb napi díjú eszköz: {elem}");

            Console.WriteLine($"\nEszközök napi kétezer forint felett:");
            var ketezerFelett = eszkozok.Where(e => e.NapiDij > 2000).OrderByDescending(e => e.NapiDij);
            foreach (var item in ketezerFelett)
            {
                Console.WriteLine(item);
            }
        }
        public static string EredmenyUzenet(KolcsonzesEredmeny eredmeny)
        {
            if (eredmeny == KolcsonzesEredmeny.Sikeres)
            {
                return "sikeres művelet";
            }
            else if (eredmeny == KolcsonzesEredmeny.MarKolcsonozve)
            {
                return "az eszköz már kölcsönözve van";
            }
            else
            {
                return "az eszköz nincs kölcsönözve";
            }
        }


    }
}
