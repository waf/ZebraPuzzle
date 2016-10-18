using Combinatorics.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZebraPuzzle
{
    class Program
    {
        public static void Main(string[] args)
        {
            // helper functions for destructure a list into anonymous types
            var ToColors = Func((IList<int> o) => new { Red = o[0], Green = o[1], Ivory = o[2], Yellow = o[3], Blue = o[4] });
            var ToNationalities = Func((IList<int> o) => new { Englishman = o[0], Spaniard = o[1], Ukranian = o[2], Japanese = o[3], Norwegian = o[4] });
            var ToDrinks = Func((IList<int> o) => new { Coffee = o[0], Tea = o[1], Milk = o[2], OJ = o[3], Water = o[4] });
            var ToCigarettes = Func((IList<int> o) => new { OldGold = o[0], Kools = o[1], Chesterfields = o[2], LuckyStrike = o[3], Parliaments = o[4] });
            var ToPets = Func((IList<int> o) => new { Dog = o[0], Snail = o[1], Fox = o[2], Horse = o[3], Zebra = o[4] });

            // set up the houses
            int[] houses = { 1, 2, 3, 4, 5 };  //1
            int first = houses[0];
            int middle = houses[2];
            var orderings = new Permutations<int>(houses).ToList();

            // solve the problem
            var answers =
                from color in orderings.Select(ToColors)
                where ImmediatelyRight(color.Green, color.Ivory) //6
                from nationality in orderings.Select(ToNationalities)
                where nationality.Englishman == color.Red &&     //2
                      nationality.Norwegian == first &&          //10
                      NextTo(nationality.Norwegian, color.Blue)  //15
                from drink in orderings.Select(ToDrinks)
                where drink.Coffee == color.Green &&             //4
                      drink.Tea == nationality.Ukranian &&       //5
                      drink.Milk == middle                       //9
                from smoke in orderings.Select(ToCigarettes)
                where smoke.Kools == color.Yellow &&             //8
                      smoke.LuckyStrike == drink.OJ &&           //13
                      nationality.Japanese == smoke.Parliaments  //14
                from pet in orderings.Select(ToPets)
                where nationality.Spaniard == pet.Dog &&         //3
                      smoke.OldGold == pet.Snail &&              //7
                      NextTo(smoke.Chesterfields, pet.Fox) &&    //12
                      NextTo(smoke.Kools, pet.Horse)             //11
                select new { drink.Water, pet.Zebra };

            var answer = answers.Single();
            Console.WriteLine($"Water drinker lives in {answer.Water} and zebra owner lives in {answer.Zebra}");
        }

        /// <summary>
        /// Is house h1 immediately right of h2?
        /// </summary>
        static bool ImmediatelyRight(int h1, int h2) => h1 - h2 == 1;

        /// <summary>
        /// Are house h1 and h2 next to each other?
        /// </summary>
        static bool NextTo(int h1, int h2) => Math.Abs(h1 - h2) == 1;

        /// <summary>
        /// Allow for type inference of Func return types.
        /// </summary>
        static Func<TIn, TOut> Func<TIn, TOut>(Func<TIn, TOut> func) => func;
    }
}
