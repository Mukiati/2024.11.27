using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using Newtonsoft.Json;

namespace _2024._11._27
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int numofalma = 0;
        string maxnevalma = "";
        int dragabbalma = 0;
        string minnevalma = "";
        int olcsobbalma = int.MaxValue;
        int borderhight = -1;
        int allborderwidth = 0;
        public MainWindow()
        {
            InitializeComponent();
            CreateTextBlock();
        }
        async void CreateTextBlock()
        {
            almak.Children.Clear();
            HttpClient client = new HttpClient();
            string url = "http://127.0.0.1:4444/alma";
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                string stringResponse = await response.Content.ReadAsStringAsync();
                List<almaclass> almalist = JsonConvert.DeserializeObject<List<almaclass>>(stringResponse);
                 numofalma = 0;
                 dragabbalma = 0;
                 olcsobbalma = int.MaxValue;
                foreach (almaclass item in almalist)
                {
                    numofalma++;
                    if (olcsobbalma > item.price)
                    {
                        olcsobbalma = item.price;
                        maxnevalma = item.type;
                        
                    }
                    if (dragabbalma < item.price)
                    {
                        dragabbalma = item.price;
                        minnevalma = item.type;
                       
                    }
                    Border oneborder = new Border();
                    almak.Children.Add(oneborder);
                    Grid onegrid = new Grid();
                    oneborder.Child = onegrid;
                   

                    RowDefinition firstrow = new RowDefinition();
                 
                    RowDefinition thirdrow = new RowDefinition();
                    ColumnDefinition firstcol = new ColumnDefinition();
                    ColumnDefinition secondtcol = new ColumnDefinition();
                    onegrid.RowDefinitions.Add(firstrow);




      
                    onegrid.RowDefinitions.Add(thirdrow);
                    onegrid.ColumnDefinitions.Add(firstcol);
                    onegrid.ColumnDefinitions.Add(secondtcol);

                    TextBlock almaneev = new TextBlock();
                    TextBlock almaaar = new TextBlock();
                    Button sell = new Button();
                    sell.Height = 20;
                    //oneblock.Text = $"Alma neve: {item.type}, alma ára: {item.price}";
                    onegrid.Children.Add(almaneev);
                    onegrid.Children.Add(almaaar);
                    onegrid.Children.Add(sell);

                
                    Grid.SetRow(almaaar,1);
                    Grid.SetRow(sell,2);

                    almaneev.Text = $"Név: {item.type}";
                    almaaar.Text = $"Ára: {item.price}";
                    sell.Content = "Eladás";
                    sell.Click += async (s, e) => {
                        HttpClient delclient = new HttpClient();
                        string delurl = "http://127.1.1.1:4444/alma";
                        try
                        {
                            var jsonObject = new
                            {
                                id = item.id
                            };
                            string jsonString = JsonConvert.SerializeObject(jsonObject);
                            HttpRequestMessage request = new HttpRequestMessage();
                            request.Method = HttpMethod.Delete;
                            request.RequestUri = new Uri(delurl);
                            request.Content = new StringContent(jsonString, Encoding.UTF8,"application/json");
                            HttpResponseMessage delresponse = await client.SendAsync(request);
                            delresponse.EnsureSuccessStatusCode();
                            almak.Children.Remove(oneborder);
                            almalist.Remove(item);
                            numofalma = almalist.Count;
                            olcsobbalma = almalist.Min(x => x.price);
                            dragabbalma = almalist.Max(x => x.price);
                            almadb.Text = numofalma + "DB";
                            //almamin.Text = olcsobbalma + "Ft"+ minnevalma;
                            //almamax.Text = dragabbalma + "Ft" + maxnevalma;
                            almamin.Text = maxnevalma;
                            almamax.Text = minnevalma;
                        }
                        catch (Exception error)
                        {

                            MessageBox.Show(error.Message);
                        }

                    };
                    oneborder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#484848"));
                    oneborder.Margin = new Thickness(5);
                    oneborder.CornerRadius = new CornerRadius(10);
                    oneborder.Padding = new Thickness(5);
                    
                    if (borderhight <0)
                    {
                       borderhight = (int)oneborder.Height;
                    }
                    allborderwidth += (int)oneborder.Width+(int)oneborder.Margin.Left+(int)oneborder.Margin.Right;

                    almaneev.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    almaaar.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }
                


                //wrappanel sor szám:
                int row = (int)almak.Height / borderhight;

                //wrappanel sor szám szélsség alapján:
                int Row = (int)Math.Ceiling(allborderwidth / almak.Width);


                //almak.Height = Row * borderhight;

                almadb.Text = numofalma + "DB";
                //almamin.Text = olcsobbalma + "Ft"+ minnevalma;
                //almamax.Text = dragabbalma + "Ft" + maxnevalma;
                almamin.Text = maxnevalma;
                almamax.Text = minnevalma;
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }

        }
       
       
        async void Addapple(object s, EventArgs e)
        {
            HttpClient client = new HttpClient();
            string url = "http://127.1.1.1:4444/alma";
            try
            {
                var jsonObject = new
                {
                    type = almanevv.Text,
                    price = almaarv.Text
                };

                string jsonData = JsonConvert.SerializeObject(jsonObject);
                StringContent data = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, data);
                response.EnsureSuccessStatusCode();
                CreateTextBlock();
            }
            catch (Exception error)
            {

                MessageBox.Show(error.Message);
            }
            //MessageBox.Show($"Alma neve: {nev.Text} , Alma ára: {ar.Text}");
        }
    }
}

