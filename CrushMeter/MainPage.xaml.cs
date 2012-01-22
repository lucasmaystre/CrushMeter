using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Security.Cryptography;
using Microsoft.Phone.Controls;

namespace CrushMeter
{
    public partial class MainPage : PhoneApplicationPage
    {
        private HMACSHA256 MACer = null;

        private string[] good = { "agreeable", "brave", "delightful", "eager",
                                  "faithful", "gentle", "happy", "jolly", "kind", "lively",
                                  "nice", "obedient", "proud", "relieved", "thankful",
                                  "victorious", "witty", "zealous" };

        private string[] med = { "mysterious", "calm", "so so", "alright", "boring", "silly" };

        private string[] bad = { "angry", "bewildered", "clumsy", "defeated", "embarrassed",
                                 "fierce", "grumpy", "helpless", "itchy", "jealous", "lazy",
                                 "nervous", "obnoxious", "panicky", "repulsive", "scary",
                                 "thoughtless", "uptight", "worried" };

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            MACer = new HMACSHA256(System.Text.Encoding.UTF8.GetBytes("Y U NO CARE ABOUT THE KEY???"));
        }

        private void calculate(object sender, RoutedEventArgs e)
        {
            // Convert to lowercase.
            string a = nameA.Text.ToLowerInvariant();
            string b = nameB.Text.ToLowerInvariant();

            if(string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) {
                return;
            }

            // Flip a and b if they're not ordered.
            if (b.CompareTo(a) < 0)
            {
                string c = a;
                a = b;
                b = c;
            }

            // HMAC it, baby!
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(a + b);
            byte[] mac = MACer.ComputeHash(bytes);

            // In C#, byte is an unsigned int between 0 and 255.
            double val = (double)mac[0] / 2.55;

            string[] adjectives;
            if (val < 40)
                adjectives = bad;
            else if (val < 60)
                adjectives = med;
            else
                adjectives = good;

            int index = (mac[1] * adjectives.Length) / 255;
            string descr = "Your relationship is : " + adjectives[index];


            // Small corner cases :)
            if ((a.Equals("anaelle") || a.Equals("anaëlle")) && b.Equals("lucas"))
            {
                val = 99.9;
                descr = "We're just awesome!!!";
            }
            else if (a.Equals("aline") && b.Equals("nicolas"))
            {
                val = 100;
                descr = "And they lived happily ever after...";
            }

            slider.Value = val / 10.0;
            slider.Opacity = 1;

            percentage.Text = string.Format("{0:0.0}", val) + " % match !";
            description.Text = descr;
        }
    }
}