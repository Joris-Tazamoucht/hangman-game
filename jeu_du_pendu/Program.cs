using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using AsciiArt;
using System.IO;

namespace jeu_du_pendu
{
    class Program
    {




        static void AfficherMot(string mot, List<char> lettres)
        {
            for (int i = 0; i < mot.Length; i++)
            {
                char lettre = mot[i];
                if (lettres.Contains(lettre))
                {
                    Console.Write(lettre + " ");

                }
                else
                {
                    Console.Write("_ ");

                }
            }
            Console.WriteLine();
        }

        static void DevinerMot(string mot)
        {
            List<char> lettresDevinees = new();
            List<char> lettresExclues = new();
            const int NB_VIES = 6;
            int viesRestantes = NB_VIES;
            while (viesRestantes > 0)
            {
                Console.WriteLine(Ascii.PENDU[NB_VIES - viesRestantes]);
                Console.WriteLine();
                AfficherMot(mot, lettresDevinees);
                Console.WriteLine();
                var lettre = DemanderUneLettre();
                Console.Clear();

                if (mot.Contains(lettre))
                {
                    lettresDevinees.Add(lettre);
                    Console.WriteLine("\t\t\t\t\t Vous venez de trouver une lettre");
                    Console.WriteLine();
                    Console.WriteLine("\t\t\t\t\t /!\\ Il vous reste : " + viesRestantes + " vies /!\\");
                    Console.WriteLine();

                    if (ToutesLettresDevinees(mot, lettresDevinees))
                    {
                        break;
                    }

                }

                else
                {
                    if (!lettresExclues.Contains(lettre))
                    {
                        viesRestantes--;
                        lettresExclues.Add(lettre);
                        Console.WriteLine();
                        Console.WriteLine("\t\t\t\t\t /!\\ Il vous reste : " + viesRestantes + " vies /!\\");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("\t\t\t\t\t La lettre a déja été tapée");
                        Console.WriteLine();
                        Console.WriteLine("\t\t\t\t\t /!\\ Il vous reste : " + viesRestantes + " vies /!\\");
                        Console.WriteLine();

                    }
                }


                if (lettresExclues.Count > 0)
                {
                    Console.WriteLine("\t\t\t\t\t Ces lettres ne sont pas dans le mot : " + String.Join(", ", lettresExclues));
                }
                Console.WriteLine();
            }

            Console.WriteLine(Ascii.PENDU[NB_VIES - viesRestantes]);

            if (viesRestantes == 0)
            {
                Console.WriteLine("Vous avez malheureusement perdu ! Le mot était : " + mot);
            }
            else
            {
                AfficherMot(mot, lettresDevinees);
                Console.WriteLine();
                Console.WriteLine("Vous avez gagné !");
            }


        }


        static bool ToutesLettresDevinees(string mot, List<char> lettres)
        {
            foreach (var lettre in lettres)
            {
                mot = mot.Replace(lettre.ToString(), "");
            }

            if (mot.Length == 0)
            {
                return true;
            }
            return false;
        }

        static string[] ChargerMots(string nomFichier)
        {
            try
            {
                return File.ReadAllLines(nomFichier);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur avec le fichier " + nomFichier + " (" + ex.Message + " )");
            }
            return null;

        }

        static char DemanderUneLettre()
        {
            while (true)
            {
                Console.Write("Rentrez une lettre : ");
                string reponse = Console.ReadLine();
                if (reponse.Length == 1)
                {
                    reponse = reponse.ToUpper();
                    return reponse[0];
                }

                Console.WriteLine("Erreur : veuillez ne rentrer qu'une seulle lettre");


            }
        }



        static bool DemanderDeRejouer()
        {
            Console.WriteLine();
            Console.WriteLine("Voulez vous rejouer ? (o/n) ? ");
            Console.WriteLine();
            char reponse = DemanderUneLettre();
            if ((reponse == 'o' || reponse == 'O'))
            {
                return true;
                Console.Clear();
            }
            else if ((reponse == 'n' || reponse == 'N'))
            {
                return false;
            }
            else
            {
                Console.WriteLine("Vous devez répondre avec o ou n");
                return DemanderDeRejouer();
            }

        }




        static void Main(string[] args)
        {
            var mots = ChargerMots("mots.txt");


            if (mots == null || (mots.Length == 0))
            {
                Console.WriteLine("La liste des mots est vide");
            }

            else
            {
                while (true)
                {
                    Random motAleatoire = new Random();
                    int nombreAleatoire = motAleatoire.Next(mots.Length);
                    string mot = mots[nombreAleatoire].Trim().ToUpper();
                    DevinerMot(mot);

                    if (!DemanderDeRejouer())
                    {
                        break;
                    }
                    Console.Clear() ;
                }
                Console.WriteLine();
                Console.WriteLine("Merci et à bientôt ! ");
            }
        }
    }
}