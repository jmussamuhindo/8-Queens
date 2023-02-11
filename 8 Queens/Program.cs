using System;
using System.Collections;
namespace _8_Queens
{   
    class Queens 
        {
        int n,ftotal=0;
        int[] f;
        bool[,] chessboard;
        int[,] genomes;
        int[,] offsprings;
        int[,] subintervals;
        int[] R;
        int[,] parent;
        int[,] randomset;
        public void run()
        {
            Random rnd = new Random();
            Console.WriteLine("How many genomes you would like to create?");
            n = Convert.ToInt32(Console.ReadLine());
            genomes = new int[n,8];
            f = new int[n];
            subintervals = new int[n, 2];
            R = new int[n];
            parent = new int[n, 8];
            randomset = new int[n, 8];
            offsprings = new int[n, 8];
            for (int k=0;k<n;k++)
            {
                Console.WriteLine("\r");
                Console.WriteLine("\r" + (k + 1 )+ ".Chessboard;");
                chessboard = new bool[8, 8];
                //generating genomes and chessboard
                for (int j=0;j<8;j++)
                {
                    int x = rnd.Next(0, 8);
                    chessboard[x,j] = true;
                    genomes[k,j] = x;
                }
               f[k]=Attack();
               printchess();              
               Console.WriteLine("\r"+ (k + 1) +". Genome;");
               prinnt(k);
               Console.WriteLine("\r");
               Console.WriteLine("\r" + "Fitness number; " + (28- f[k]) + "\n" + "Attack Number; " + f[k]);             
               f[k] = 28 - f[k]; 
               ftotal = ftotal + f[k];
               int prev = 0;
            //setting subintervals of first generation of genomes
               if (k != 0)
                    prev = subintervals[k - 1, 1];
              
                subintervals[k, 0] = prev;
                subintervals[k, 1] = prev + f[k];
            }
            int attak = 0 , count = 0 , roulet=0;
            //running mutation untill finding non-attacking genome
            while(attak!=28)
            {
                roulet++; 
                //roulette wheel
                for (int j = 0; j < n; j++)
                {
                    R[j] =Convert.ToInt32( Convert.ToDecimal(subintervals[n - 1, 1]) *( Convert.ToDecimal(rnd.Next(1, 101)) / 100));
                }
                //Mating Pool
                for (int m = 0; m < n; m++)
                {
                    bool mating = false;
                    int ma = 0;
                    while (mating == false)
                    {
                        if (Convert.ToInt32(R[m]) >= Convert.ToInt32(subintervals[ma, 0]) & Convert.ToInt32(R[m]) <= Convert.ToInt32(subintervals[ma, 1]))
                        {
                            mating = true;
                            for (int u = 0; u < 8; u++)
                                parent[m, u] = genomes[ma, u];
                        }
                        ma++;
                    }
                }               
                for (int l=0;l<n;l++)
                {
                    for (int p=0;p<8;p++)
                    {
                        offsprings[l, p] = parent[l, p];
                    }
                }
                //Crossover
                for (int o=0; o<n;o+=2)
                {                                        
                    int r1 = rnd.Next(1, 8);
                    int r2 = rnd.Next(1, 8);
                    if (r1 - r2 == 0 || r2 - r1 == 0)
                        continue; 
                    int res;
                    if (r1 > r2)
                    { 
                        res = r1 - r2;
                        for (int rr = 0; rr < res; rr++)
                        {
                            int temp = offsprings[o, r2 +rr];
                            offsprings[o, r2+rr] = offsprings[o + 1, r2+rr];
                            offsprings[o + 1, r2+rr] = temp;
                        }
                    }
                    else
                    {
                        res = r2 - r1;
                        for (int rr = 0; rr < res; rr++)
                        {
                            int temp = offsprings[o, r1 + rr];
                            offsprings[o, r1 + rr] = offsprings[o + 1, r1 + rr];
                            offsprings[o + 1, r1 + rr] = temp;
                        }
                    }
                }
                //Mutation
                for (int k=0;k<n;k++)
                {
                    for(int j=0;j<8;j++)
                    {
                        randomset[k, j] = rnd.Next(1, 16);
                        if (randomset[k,j]==1)
                        {
                            offsprings[k, j] = rnd.Next(0, 8);
                        }
                        genomes[k, j] = offsprings[k, j]; 
                    }
                    count++;
                }
                for (int k = 0; k < n; k++)
                {
                    chessboard = new bool[8, 8];
                    int[] gen = new int[8];
                    for (int x=0;x<8;x++)
                    {
                        gen[x] = genomes[k,x];
                    }

                    for (int j = 0; j < 8; j++)
                    {
                        int b = gen[j];
                        chessboard[b , j] = true;    
                    }
                    f[k] = Attack();                
                    f[k] = 28 - f[k];
                    //Checking if there is 0 attack = 28 fitness number
                    if (f[k]==28)
                    {
                        attak = 28;
                        Console.WriteLine("\r");
                        Console.WriteLine("You found the non-attacking genome after; " + (count - n + k + 1) + " attempt ");
                        prinnt(k);
                        Console.WriteLine("\r");
                        printchess();
                        break;
                    }
                    ftotal = ftotal + f[k];
                    int prev = 0;
                    //New subintervals for mutated genomes
                    if (k != 0)
                        prev = subintervals[k - 1, 1];
                    subintervals[k, 0] = prev;
                    subintervals[k, 1] = prev + f[k];
                }
            }    
        }
        //Printing genome as numbers
        public void prinnt(int c)
        {           
            for (int j = 0; j < 8; j++)
            {
                string comma = ",";
                if (j==7)
                {
                    comma = " ";
                }               
                Console.Write(genomes[c, j] + 1 + comma);
            }
        }
        //Printing Chessboard
        public void printchess()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int k = 0; k < 8; k++)
                {
                    Console.Write(chessboard[i, k] ? "O" : "x");
                    if (k == 7) Console.WriteLine("\r");
                }
                if (i == 7) Console.WriteLine("\r");
            }
        }
        //Checking how many attacks on generated genome(chessboard)
        public int Attack()
        {   
            int result = 0;        
            for (int i = 0; i < 7; i++)
            {
                for (int k = 0; k < 8; k++)
                {
                   if (chessboard[k,i] == true)
                    {
                        int a = k;
                        int b = i;
                        while (b!=7)
                        {
                            if (chessboard[a, b + 1] == true)
                                result++;
                            b++;                           
                        }                        
                        a = k;
                        b = i;
                        while (a!=7 & b!=7)
                        {
                            if (chessboard[a + 1, b +1] == true)
                                result++;
                            a++;
                            b++;
                        }
                        a = k;
                        b = i;
                        while (a != 0 & b != 7)
                        {
                            if (chessboard[a + -1, b + 1] == true)
                                result++;
                            a--;
                            b++;
                        }
                    }
                }               
            }
            return result;
        }
    }
class Program
    {
        static void Main(string[] args)
        {
            Queens r = new Queens();
            r.run();
        }
    } 
}
