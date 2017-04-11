using System;

namespace BallsDeep
{
    class Program
    {
        const double epsilon0 = 8.854187817620e-12;
        const double pi = Math.PI;
        

        const double precision = double.Epsilon;
        const int N = 250;

        static Ball[] balls = new Ball[2];

        static double distance;
        static double Q;
        static double A;
        static double U;

        static void Main(string[] args)
        {
            balls[0] = new Ball();
            balls[1] = new Ball();
            Console.WriteLine("Prozraď mi poloměr první koule [m]:");
            balls[0].a = double.Parse(Console.ReadLine());
            Console.WriteLine("Prozraď mi potenciál první koule [V]:");
            balls[0].U = double.Parse(Console.ReadLine());
            Console.WriteLine("Prozraď mi poloměr druhé koule [m]:");
            balls[1].a = double.Parse(Console.ReadLine());
            Console.WriteLine("Prozraď mi potenciál druhé koule [V]:");
            balls[1].U = double.Parse(Console.ReadLine());

            Console.WriteLine("Prozraď mi vzdálenost koulí [m]:");
            distance = double.Parse(Console.ReadLine());

            Console.WriteLine("Počítám...");
            compute();
            Console.WriteLine("Sčítám...");
            Q = sum();
            Koule();
            Console.WriteLine("Celkový Q: {0:R}", Q);
            Console.WriteLine("Celkový U: {0:R}", U);
            Console.ReadLine();
        }

        static void next(double a, double q, double d, out double Q_out, out double D_out)
        {
            Q_out = -(a / d) * q;
            D_out = a * a / d;
        }

        static void compute()
        {
            balls[0].q = new double[N];
            balls[1].q = new double[N];
            balls[0].d = new double[N];
            balls[1].d = new double[N];
            balls[0].q[0] = balls[0].U * balls[0].a * 4 * pi * epsilon0;
            Console.WriteLine("Q0 = " + balls[0].q[0]);
            balls[1].q[0] = balls[1].U * balls[1].a * 4 * pi * epsilon0;
            Console.WriteLine("Q0' = " + balls[1].q[0]);
            balls[0].d[0] = 0.0;
            balls[1].d[0] = distance;
            for(int i = 1; i < N; i++)
            {
                double Q;
                double D;
                next(balls[0].a, balls[1].q[i - 1], balls[1].d[i - 1], out Q, out D);
                balls[0].q[i] = Q;
                balls[0].d[i] = D;
                next(balls[1].a, balls[0].q[i - 1], Math.Abs(distance - balls[0].d[i - 1]), out Q, out D);
                balls[1].q[i] = Q;
                balls[1].d[i] = distance - D;
                if(i < 6)
                {
                    Console.WriteLine("\nQ" + i + " = " + balls[0].q[i]);
                    Console.WriteLine("Q" + i + "' = " + balls[1].q[i]);
                    Console.WriteLine("D" + i + " = " + balls[0].d[i]);
                    Console.WriteLine("D" + i + "' = " + balls[1].d[i]);
                }
            }
        }

        static double sum()
        {
            double sum = 0;
            double pre_sum = 0;
            for (int i = 0; i < N; i++)
            {
                sum += balls[0].q[i] + balls[1].q[i];
                if(i >= 50 && Math.Abs(sum - pre_sum) <= precision)
                {
                    Console.WriteLine("Precision reached!");
                    break;
                }
                pre_sum = sum;
            }
            return sum;
        }

        static void Koule()
        {
            double V1 = balls[0].a*balls[0].a*balls[0].a;
            double V2 =  balls[1].a*balls[1].a*balls[1].a;
            double V = V1 + V2;
            A = Math.Pow(V, 1.0/3.0);
            U = 1.0 / 4.0 / pi / epsilon0 * Q / A;
        }
    }

    struct Ball
    {
        public double U;
        public double a;
        public double[] q;
        public double[] d;
    }
}
