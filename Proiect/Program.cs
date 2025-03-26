using System;
using System.Collections.Generic;
using System.IO;

namespace TargDeMasini1
{
    class Program
    {
        static void Main()
        {
            TargAuto targ = new TargAuto();
            int optiune;

            do
            {
                Console.WriteLine("\n--- Meniu Targ Auto ---");
                Console.WriteLine("1. Adauga masina");
                Console.WriteLine("2. Afiseaza masini");
                Console.WriteLine("3. Cauta masina");
                Console.WriteLine("4. Sterge masina");
                Console.WriteLine("5. Salveaza masini in fisier");
                Console.WriteLine("6. Citeste masini din fisier");
                Console.WriteLine("7. Iesire");
                Console.Write("Alege o optiune: ");

                if (int.TryParse(Console.ReadLine(), out optiune))
                {
                    switch (optiune)
                    {
                        case 1:
                            AdaugaMasina(targ);
                            break;
                        case 2:
                            targ.AfiseazaMasini();
                            break;
                        case 3:
                            CautaMasina(targ);
                            break;
                        case 4:
                            StergeMasina(targ);
                            break;
                        case 5:
                            targ.SalveazaInFisier();
                            Console.WriteLine("Masinile au fost salvate in fisier.");
                            break;
                        case 6:
                            targ.CitesteDinFisier();
                            Console.WriteLine("Masinile au fost citite din fisier.");
                            break;
                        case 7:
                            Console.WriteLine("Iesire din program.");
                            break;
                        default:
                            Console.WriteLine("Optiune invalida! Incearca din nou.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Te rog sa introduci un numar valid.");
                }

            } while (optiune != 7);
        }

        static void AdaugaMasina(TargAuto targ)
        {
            Console.Write("Introdu marca: ");
            string marca = Console.ReadLine();

            Console.Write("Introdu modelul: ");
            string model = Console.ReadLine();

            Console.Write("Introdu anul de fabricatie: ");
            string input = Console.ReadLine();
            int anFabricatie;
            decimal pret;
            try
            {
                anFabricatie = int.Parse(input);
            }
            catch
            {
                Console.WriteLine("An invalid!");
                return;
            }

            Console.Write("Introdu pretul: ");
            string inp = Console.ReadLine();

            try
            {
                pret = decimal.Parse(inp);
            }
            catch
            {
                Console.WriteLine("Pret invalid!");
                return;
            }

            Console.WriteLine("Introdu tipul de combustibil (0: Benzina, 1: Motorina, 2: Electric, 3: Hibrid): ");
            if (!Enum.TryParse(Console.ReadLine(), out TipCombustibil combustibil))
            {
                Console.WriteLine("Tip de combustibil invalid!");
                return;
            }

            targ.AdaugaMasina(new Masina(marca, model, anFabricatie, pret, combustibil));
            Console.WriteLine("Masina adaugata cu succes!");
        }

        static void CautaMasina(TargAuto targ)
        {
            Console.Write("Introdu marca masinii pe care o cauti: ");
            string marca = Console.ReadLine();

            targ.CautaMasina(marca);
        }

        static void StergeMasina(TargAuto targ)
        {
            Console.Write("Introdu marca masinii pe care vrei sa o stergi: ");
            string marca = Console.ReadLine();

            targ.StergeMasina(marca);
        }
    }

    class Masina
    {
        public string Marca { get; set; }
        public string Model { get; set; }
        public int AnFabricatie { get; set; }
        public decimal Pret { get; set; }
        public TipCombustibil Combustibil { get; set; }

        public Masina(string marca, string model, int anFabricatie, decimal pret, TipCombustibil combustibil)
        {
            Marca = marca;
            Model = model;
            AnFabricatie = anFabricatie;
            Pret = pret;
            Combustibil = combustibil;
        }
    }

    class TargAuto
    {
        private List<Masina> masini = new List<Masina>();
        private const string filePath = @"C:\Users\Lenovo\Desktop\a\date.txt";

        public void AdaugaMasina(Masina masina)
        {
            masini.Add(masina);
        }

        public void AfiseazaMasini()
        {
            if (masini.Count == 0)
            {
                Console.WriteLine("Nu exista masini inregistrate.");
                return;
            }

            Console.WriteLine("\n--- Lista Masini ---");
            foreach (var masina in masini)
            {
                Console.WriteLine($"{masina.Marca} {masina.Model}, An: {masina.AnFabricatie}, Pret: {masina.Pret} EUR, Combustibil: {masina.Combustibil}");
            }
        }

        public void CautaMasina(string marca)
        {
            var rezultate = masini.FindAll(m => m.Marca.Equals(marca, StringComparison.OrdinalIgnoreCase));

            if (rezultate.Count == 0)
            {
                Console.WriteLine("Nu a fost gasita nicio masina cu aceasta marca.");
                return;
            }

            Console.WriteLine("\n--- Masini Gasite ---");
            foreach (var masina in rezultate)
            {
                Console.WriteLine($"{masina.Marca} {masina.Model}, An: {masina.AnFabricatie}, Pret: {masina.Pret} EUR, Combustibil: {masina.Combustibil}");
            }
        }

        public void StergeMasina(string marca)
        {
            var masina = masini.Find(m => m.Marca.Equals(marca, StringComparison.OrdinalIgnoreCase));
            if (masina != null)
            {
                masini.Remove(masina);
                Console.WriteLine("Masina a fost stearsa.");
            }
            else
            {
                Console.WriteLine("Masina nu a fost gasita.");
            }
        }

        public void SalveazaInFisier()
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var masina in masini)
                {
                    writer.WriteLine($"{masina.Marca},{masina.Model},{masina.AnFabricatie},{masina.Pret},{masina.Combustibil}");
                }
            }
        }

        public void CitesteDinFisier()
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Fisierul nu exista.");
                return;
            }

            masini.Clear();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 5 &&
                        int.TryParse(parts[2], out int anFabricatie) &&
                        decimal.TryParse(parts[3], out decimal pret) &&
                        Enum.TryParse(parts[4], out TipCombustibil combustibil))
                    {
                        masini.Add(new Masina(parts[0], parts[1], anFabricatie, pret, combustibil));
                    }
                }
            }
        }
    }

    enum TipCombustibil
    {
        Benzina,
        Motorina,
        Electric,
        Hibrid
    }
}
