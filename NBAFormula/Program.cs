using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MathNet.Numerics;

namespace NBAFormulaFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection sqlcon = new SqlConnection(@"Data Source=ANDREWSLAPTOP;Initial Catalog=NBAStats;Integrated Security=True");

            double[] test1 = new double[1200];
            double[] test2 = new double[1200];
            double bestMSE = 10000000000000;

            for (double i = 78; i <= 82; i += .1)
            {
                for (double j = 0; j <= 1; j += .1)
                {
                    string query = $"select ({i}*sum(WS_48*(cast(MP as float)/3936))+{j}*sum(BPM*(cast(MP as float)/3936))) as TeamEstWins, " +
                        "Wins " +
                        "from advancedstats, standings " +
                        "where advancedstats.year = standings.year " +
                        "and advancedstats.team = standings.team " +
                        "group by wins, standings.team, standings.year;";


                    SqlDataAdapter sda = new SqlDataAdapter(query, sqlcon);
                    DataTable dtbl = new DataTable();
                    sda.Fill(dtbl);

                    int z = 0;

                    foreach (DataRow row in dtbl.Rows)
                    {
                        test1[z] = double.Parse(row["TeamEstWins"].ToString());
                        test2[z] = double.Parse(row["Wins"].ToString());

                        z++;
                    }

                    double mse = Distance.MSE(test1, test2);

                    if (mse < bestMSE)
                    {
                        bestMSE = mse;
                        Console.Clear();
                        Console.WriteLine(bestMSE + " " + i + " " + j);
                    }
                }

            }
        }
    }
}