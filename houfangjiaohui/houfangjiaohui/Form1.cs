using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace houfangjiaohui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "-86.15";
            textBox2.Text = "-68.99";
            textBox3.Text = "36589.41";
            textBox4.Text = "25273.32";
            textBox5.Text = "2195.17";
            textBox6.Text = "-53.4";
            textBox7.Text = "82.21";
            textBox8.Text = "37631.08";
            textBox9.Text = "31324.51";
            textBox10.Text = "728.69";
            textBox11.Text = "-14.78";
            textBox12.Text = "-76.63";
            textBox13.Text = "39100.97";
            textBox14.Text = "24934.98";
            textBox15.Text = "2386.50";
            textBox16.Text = "10.46";
            textBox17.Text = "64.43";
            textBox18.Text = "40426.54";
            textBox19.Text = "30319.81";
            textBox20.Text = "757.31";
        }
        const double m = 15000;
        const double f = 153.24 / 1000;
        const double x0 = 0;
        const double y0 = 0;
        class MatrixOperator
        {
            //矩阵加法
            public static double[,] MatrixAdd(double[,] Ma, double[,] Mb)
            {
                int m = Ma.GetLength(0);
                int n = Ma.GetLength(1);
                int m2 = Mb.GetLength(0);
                int n2 = Mb.GetLength(1);

                if ((m != m2) || (n != n2))
                {
                    Exception myException = new Exception("数组维数不匹配");
                    throw myException;
                }
                double[,] c = new double[m, n];
                for (int i = 0; i < m; i++)
                    for (int j = 0; j < n; j++)
                        c[i, j] = Ma[i, j] + Mb[i, j];
                return c;
            }

            //矩阵减法
            public static double[,] MatrixSub(double[,] Ma, double[,] Mb)
            {
                int m = Ma.GetLength(0);
                int n = Ma.GetLength(1);
                int m2 = Mb.GetLength(0);
                int n2 = Mb.GetLength(1);
                if ((m != m2) || (n != n2))
                {
                    Exception myException = new Exception("数组维数不匹配");
                    throw myException;
                }
                double[,] c = new double[m, n];
                double[,] a = Ma;
                double[,] b = Mb;
                for (int i = 0; i < m; i++)
                    for (int j = 0; j < n; j++)
                        c[i, j] = a[i, j] - b[i, j];
                return c;
            }

            //矩阵乘法
            public static double[,] MatrixMulti(double[,] Ma, double[,] Mb)
            {
                int m = Ma.GetLength(0);
                int n = Ma.GetLength(1);
                int m2 = Mb.GetLength(0);
                int n2 = Mb.GetLength(1);
                if (n != m2)
                {
                    Exception myException = new Exception("数组维数不匹配");
                    throw myException;
                }

                double[,] c = new double[m, n2];
                double[,] a = Ma;
                double[,] b = Mb;

                for (int i = 0; i < m; i++)
                    for (int j = 0; j < n2; j++)
                    {
                        c[i, j] = 0;
                        for (int k = 0; k < n; k++)
                            c[i, j] += a[i, k] * b[k, j];
                    }
                return c;

            }

            //矩阵数乘
            public static double[,] MatrixSimpleMulti(double k, double[,] Ma)
            {
                int m = Ma.GetLength(0);
                int n = Ma.GetLength(1);
                double[,] c = new double[m, n];
                for (int i = 0; i < m; i++)
                    for (int j = 0; j < n; j++)
                        c[i, j] = Ma[i, j] * k;
                return c;
            }

            //矩阵转置
            public static double[,] MatrixTrans(double[,] MatrixOrigin)
            {
                int m = MatrixOrigin.GetLength(0);
                int n = MatrixOrigin.GetLength(1);

                double[,] c = new double[n, m];
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < m; j++)
                        c[i, j] = MatrixOrigin[j, i];
                return c;
            }

            //矩阵求逆（伴随矩阵法）
            public static double[,] MatrixInvByCom(double[,] Ma)
            {
                double d = MatrixOperator.MatrixDet(Ma);
                if (d == 0)
                {
                    Exception myException = new Exception("没有逆矩阵");
                    throw myException;
                }
                double[,] Ax = MatrixOperator.MatrixCom(Ma);
                double[,] An = MatrixOperator.MatrixSimpleMulti((1.0 / d), Ax);
                return An;
            }
            //对应行列式的代数余子式矩阵
            public static double[,] MatrixSpa(double[,] Ma, int ai, int aj)
            {
                int m = Ma.GetLength(0);
                int n = Ma.GetLength(1);
                if (m != n)
                {
                    Exception myException = new Exception("矩阵不是方阵");
                    throw myException;
                }
                int n2 = n - 1;
                double[,] b = new double[n2, n2];

                //左上
                for (int i = 0; i < ai; i++)
                    for (int j = 0; j < aj; j++)
                    {
                        b[i, j] = Ma[i, j];
                    }
                //右下
                for (int i = ai; i < n2; i++)
                    for (int j = aj; j < n2; j++)
                    {
                        b[i, j] = Ma[i + 1, j + 1];
                    }
                //右上
                for (int i = 0; i < ai; i++)
                    for (int j = aj; j < n2; j++)
                    {
                        b[i, j] = Ma[i, j + 1];
                    }
                //左下
                for (int i = ai; i < n2; i++)
                    for (int j = 0; j < aj; j++)
                    {
                        b[i, j] = Ma[i + 1, j];
                    }
                //符号位
                if ((ai + aj) % 2 != 0)
                {
                    for (int i = 0; i < n2; i++)
                        b[i, 0] = -b[i, 0];

                }
                return b;

            }

            //矩阵的行列式,矩阵必须是方阵
            public static double MatrixDet(double[,] Ma)
            {
                int m = Ma.GetLength(0);
                int n = Ma.GetLength(1);
                if (m != n)
                {
                    Exception myException = new Exception("数组维数不匹配");
                    throw myException;
                }
                double[,] a = Ma;
                if (n == 1) return a[0, 0];

                double D = 0;
                for (int i = 0; i < n; i++)
                {
                    D += a[1, i] * MatrixDet(MatrixSpa(Ma, 1, i));
                }
                return D;
            }

            //矩阵的伴随矩阵
            public static double[,] MatrixCom(double[,] Ma)
            {
                int m = Ma.GetLength(0);
                int n = Ma.GetLength(1);
                double[,] c = new double[m, n];
                double[,] a = Ma;

                for (int i = 0; i < m; i++)
                    for (int j = 0; j < n; j++)
                        c[i, j] = MatrixDet(MatrixSpa(Ma, j, i));

                return c;
            }
        }
        class file
        {
            public static void putin(double[,] num, string path_)
            {

                FileStream fp = File.Create(path_);
                StreamWriter sw = new StreamWriter(fp, Encoding.Default);
                for (int i = 0; i < num.GetLength(0); i++)
                {

                    //sw.WriteLine("{0}    {1}\t{2}",v[0],v[1],v[2]);
                    //sw.WriteLine(v[0].ToString() + '\t' + v[1].ToString() + '\t' + v[2].ToString());
                    for (int j = 0; j < num.GetLength(1); j++)
                    {
                        sw.Write(num[i, j].ToString("f4") + "\t");
                    }
                    sw.WriteLine();
                }
                sw.Close();
                fp.Close();

            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            double[] x = new double[4];
            double[] y = new double[4];
            double[] X = new double[4];
            double[] Y = new double[4];
            double[] Z = new double[4];
            double X0 = new double();
            double Y0 = new double();
            double Z0 = new double();
            double r0 = new double();
            double w0 = new double();
            double k0 = new double();
            bool condition = true;
            double[,] jsz = new double[6, 1];//近似值
            double[,] l = new double[8, 1];
            x[0] = double.Parse(textBox1.Text) / 1000;
            y[0] = double.Parse(textBox2.Text) / 1000;
            X[0] = double.Parse(textBox3.Text);
            Y[0] = double.Parse(textBox4.Text);
            Z[0] = double.Parse(textBox5.Text);
            x[1] = double.Parse(textBox6.Text) / 1000;
            y[1] = double.Parse(textBox7.Text) / 1000;
            X[1] = double.Parse(textBox8.Text);
            Y[1] = double.Parse(textBox9.Text);
            Z[1] = double.Parse(textBox10.Text);
            x[2] = double.Parse(textBox11.Text) / 1000;
            y[2] = double.Parse(textBox12.Text) / 1000;
            X[2] = double.Parse(textBox13.Text);
            Y[2] = double.Parse(textBox14.Text);
            Z[2] = double.Parse(textBox15.Text);
            x[3] = double.Parse(textBox16.Text) / 1000;
            y[3] = double.Parse(textBox17.Text) / 1000;
            X[3] = double.Parse(textBox18.Text);
            Y[3] = double.Parse(textBox19.Text);
            Z[3] = double.Parse(textBox20.Text);
            for (int i = 0; i < X.GetLength(0); i++)
            {
                X0 += X[i]; Y0 += Y[i]; Z0 += Z[i];
            }
            X0 = X0 / X.GetLength(0);
            Y0 = Y0 / Y.GetLength(0);
            Z0 = /*m * f;*/Z0 / Z.GetLength(0) + m * f;
            jsz[0, 0] = X0;
            jsz[1, 0] = Y0;
            jsz[2, 0] = Z0;
            jsz[3, 0] = r0;
            jsz[4, 0] = w0;
            jsz[5, 0] = k0;
            int time = 0;
            while (condition)
            {
                time++;
                double[,] v = new double[6, 1];
                double[,] b = new double[8, 6];
                double a1 = Math.Cos(r0) * Math.Cos(k0) - Math.Sin(r0) * Math.Sin(w0) * Math.Sin(k0);
                double a2 = -Math.Cos(r0) * Math.Sin(k0) - Math.Sin(r0) * Math.Sin(w0) * Math.Cos(k0);
                double a3 = -Math.Sin(r0) * Math.Cos(w0);
                double b1 = Math.Cos(w0) * Math.Sin(k0);
                double b2 = Math.Cos(w0) * Math.Cos(k0);
                double b3 = -Math.Sin(w0);
                double c1 = Math.Sin(r0) * Math.Cos(k0) + Math.Cos(r0) * Math.Sin(w0) * Math.Sin(k0);
                double c2 = -Math.Sin(r0) * Math.Sin(k0) + Math.Cos(r0) * Math.Sin(w0) * Math.Cos(k0);
                double c3 = Math.Cos(r0) * Math.Cos(w0);
                for (int i = 0; i < x.GetLength(0); i++)
                {
                    double xx = a1 * (X[i] - jsz[0, 0]) + b1 * (Y[i] - jsz[1, 0]) + c1 * (Z[i] - jsz[2, 0]);
                    double yy = a2 * (X[i] - jsz[0, 0]) + b2 * (Y[i] - jsz[1, 0]) + c2 * (Z[i] - jsz[2, 0]);
                    double zz = a3 * (X[i] - jsz[0, 0]) + b3 * (Y[i] - jsz[1, 0]) + c3 * (Z[i] - jsz[2, 0]);
                    b[2 * i, 0] = (a1 * f + a3 * (x[i] - x0)) / zz;
                    b[2 * i, 1] = (b1 * f + b3 * (x[i] - x0)) / zz;
                    b[2 * i, 2] = (c1 * f + c3 * (x[i] - x0)) / zz;
                    b[2 * i, 3] = (y[i] - y0) * Math.Sin(w0) - (((x[i] - x0) / f) * ((x[i] - x0) * Math.Cos(k0) - (y[i] - y0) * Math.Sin(k0)) + f * Math.Cos(k0)) * Math.Cos(w0);
                    b[2 * i, 4] = -f * Math.Sin(k0) - ((x[i] - x0) / f )* ((x[i] - x0) * Math.Sin(k0) + (y[i] - y0) * Math.Cos(k0));
                    b[2 * i, 5] = (y[i] - y0);
                    b[2 * i + 1, 0] = (a2 * f + a3 * (y[i] - y0)) / zz;
                    b[2 * i + 1, 1] = (b2 * f + b3 * (y[i] - y0)) / zz;
                    b[2 * i + 1, 2] = (c2 * f + c3 * (y[i] - y0)) / zz;
                    b[2 * i + 1, 3] = -(x[i]-x0) * Math.Sin(w0) - (((y[i]-y0) / f) * ((x[i]-x0 )* Math.Cos(k0) - (y[i]-y0) * Math.Sin(k0)) - f * Math.Sin(k0)) * Math.Cos(w0);
                    b[2 * i + 1, 4] = -f * Math.Cos(k0) - ((y[i] - y0) / f )* ((x[i] - x0) * Math.Sin(k0) + (y[i] - y0) * Math.Cos(k0));
                    b[2 * i + 1, 5] = -(x[i] - x0);
                    l[2 * i, 0] = x[i] - (-f * xx / zz);
                    l[2 * i + 1, 0] = y[i] - (-f * yy / zz);
                    /*double xx = -f*(a1 * (X[i] - jsz[0, 0]) + b1 * (Y[i] - jsz[1, 0]) + c1 * (Z[i] - jsz[2, 0])) / (a3 * (X[i] - jsz[0, 0]) + b3 * (Y[i] - jsz[1, 0]) + c3 * (Z[i] - jsz[2, 0]));
                    double yy = -f*(a2 * (X[i] - jsz[0, 0]) + b2 * (Y[i] - jsz[1, 0]) + c2 * (Z[i] - jsz[2, 0])) / (a3 * (X[i] - jsz[0, 0]) + b3 * (Y[i] - jsz[1, 0]) + c3 * (Z[i] - jsz[2, 0]));
                    b[2 * i, 0] =-f/(jsz[2,0]-Z[i]);
                    b[2 * i, 1] = 0;
                    b[2 * i, 2] = -xx / (jsz[2, 0] - Z[i]);
                    b[2 * i, 3] = -f * (1 + Math.Pow(xx, 2) / Math.Pow(f, 2));
                    b[2 * i, 4] = -xx * yy / f;
                    b[2 * i, 5] = yy;
                    b[2 * i + 1, 0] = 0;
                    b[2 * i + 1, 1] = -f / (jsz[2, 0] - Z[i]);
                    b[2 * i + 1, 2] = -yy / (jsz[2, 0] - Z[i]);
                    b[2 * i + 1, 3] = -xx * yy/ f;
                    b[2 * i + 1, 4] = -f * (1 + Math.Pow(yy, 2) / Math.Pow(f, 2));
                    b[2 * i + 1, 5] =-xx;
                    l[2 * i, 0] = x[i] - xx;
                    l[2 * i + 1, 0] = y[i] - yy;*/
                    
                }
                double[,] bt = MatrixOperator.MatrixTrans(b);
               // file.putin(bt, "D:\\Users\\wangxing\\Desktop\\wx\\c#\\houfangjiaohui\\test\\bt");
                //file.putin(l, "D:\\Users\\wangxing\\Desktop\\wx\\c#\\houfangjiaohui\\test\\l");
                double[,] nbb = MatrixOperator.MatrixMulti(bt, b);
                double[,] w = MatrixOperator.MatrixMulti(bt, l);
                double[,] nbb_ = MatrixOperator.MatrixInvByCom(nbb);
                v = MatrixOperator.MatrixMulti(nbb_, w);
                jsz = MatrixOperator.MatrixAdd(jsz, v);
                if (Math.Abs(v[0, 0]) > 0.001 || Math.Abs(v[1, 0]) > 0.001 || Math.Abs(v[2, 0]) > 0.001 || Math.Abs(v[3, 0]) > 3 * Math.Pow(10, -5) || Math.Abs(v[4, 0]) > 3 * Math.Pow(10, -5) || Math.Abs(v[5, 0]) > 3 * Math.Pow(10, -5))
                {
                    condition = true;
                }
                else
                {
                    double[,] mi = new double[6, 1];
                    double[,] vv =MatrixOperator.MatrixAdd(MatrixOperator.MatrixMulti(b, v),l);
                    double vv2 = MatrixOperator.MatrixMulti(MatrixOperator.MatrixTrans(vv), vv)[0,0];
                    double m0 = Math.Pow(vv2/(2*x.GetLength(0)-6), 0.5);
                    for (int i = 0; i < mi.GetLength(0); i++)
                    {
                        mi[i, 0] = Math.Pow(nbb_[i, i], 0.2)*m0;

                    }
                    textBox37.Text = mi[0, 0].ToString("f5");
                    textBox38.Text = mi[1, 0].ToString("f5");
                    textBox39.Text = mi[2, 0].ToString("f5");
                    textBox40.Text = mi[3, 0].ToString("f5");
                    textBox41.Text = mi[4, 0].ToString("f5");
                    textBox42.Text = mi[5, 0].ToString("f5");
                    condition = false;
                    textBox25.Text = a1.ToString("f5");
                    textBox26.Text = a2.ToString("f5");
                    textBox27.Text = a3.ToString("f5");
                    textBox28.Text = b1.ToString("f5");
                    textBox29.Text = b2.ToString("f5");
                    textBox30.Text = b3.ToString("f5");
                    textBox31.Text = c1.ToString("f5");
                    textBox32.Text = c2.ToString("f5");
                    textBox33.Text = c3.ToString("f5");
                    textBox21.Text = jsz[0, 0].ToString("f2");
                    textBox22.Text = jsz[1, 0].ToString("f2");
                    textBox23.Text = jsz[2, 0].ToString("f2");
                    textBox34.Text = jsz[3, 0].ToString("f5");
                    textBox35.Text = jsz[4, 0].ToString("f5");
                    textBox36.Text = jsz[5, 0].ToString("f5");
                    textBox24.Text = Convert.ToString(time);
                }
                r0 = jsz[3, 0];
                w0 = jsz[4, 0];
                k0=jsz[5,0];
            }
        }
    }
}
