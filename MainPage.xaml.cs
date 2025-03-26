namespace zapalki
{
    public partial class MainPage : ContentPage
    {
        private string[] liczbaZapalekArray = { "15", "16", "17", "18", "19", "20", "losowa" };
        private int liczbaZapalek;
        private Random random = new Random();

        public MainPage()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            liczbaZapalekPicker.ItemsSource = liczbaZapalekArray;
            liczbaZapalekPicker.SelectedIndex = 0; // Domyślna wartość
        }

        private void NewGame(object sender, EventArgs e)
        {
            string wyborZapalek = liczbaZapalekPicker.SelectedItem.ToString();
            liczbaZapalek = (wyborZapalek == "losowa") ? random.Next(15, 22) : int.Parse(wyborZapalek);
            liczbaZapalekLbl.IsVisible = true;
            liczbaZapalekLbl.Text = "Pozostało zapałek: " + liczbaZapalek;
            
            logsScrollView.Children.Clear();
            imageContainer.Children.Clear();
            buttonsContainer.Children.Clear();
            // Generowanie zapałek jako obrazków
            for (int i = 0; i < liczbaZapalek; i++)
            {
                var zapalka = new ImageButton
                {
                    Source = "zapalka.png",
                    WidthRequest = 20,
                    HeightRequest = 70
                };
                imageContainer.Children.Add(zapalka);
            }

            // Generowanie przycisków do wyboru ilości zapałek
            for (int i = 1; i <= 3; i++)
            {
                var button = new Button
                {
                    Text = $"Weź {i} zapałkę",
                    BackgroundColor = Colors.LightGoldenrodYellow,
                    BorderWidth = 1,
                    TextColor = Colors.Black
                };
                button.Clicked += Button_Clicked;
                button.CommandParameter = i; // Przechowuje liczbę zapałek do zabrania
                buttonsContainer.Children.Add(button);
            }
            var Label = new Label
            {
                Text = "Utworzono nową grę, liczba zapałek: " + liczbaZapalek,
                FontSize = 15,
                TextColor = Colors.Black
            };
            UpdateGameState(Label);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
        
            var button = (Button)sender;
            int zapalkiDoZabrania = (int)button.CommandParameter;

            if (liczbaZapalek >= zapalkiDoZabrania)
            {
                liczbaZapalek -= zapalkiDoZabrania;
                var Label = new Label
                {
                    Text = "Gracz zabrał: " + zapalkiDoZabrania + " zapałek",
                    FontSize = 15,
                    TextColor = Colors.Black
                };
                UpdateGameState(Label);

                if (CheckGameOver("Gracz")) return;
                await Task.Delay(2000); 
                ComputerMove();
            }
        }

        private void ComputerMove()
        {
            int zapalkiDoZabrania = random.Next(1,3); 
            liczbaZapalek -= zapalkiDoZabrania;
            var Label = new Label
            {
                Text = "Komputer zabrał: " + zapalkiDoZabrania + " zapałek",
                FontSize = 15,
                TextColor = Colors.Black
            };

            UpdateGameState(Label);
            CheckGameOver("Komputer");
           

        }

        private void UpdateGameState(Label label)
        {
            imageContainer.Children.Clear();
            for (int i = 0; i < liczbaZapalek; i++)
            {
                var zapalka = new Image
                {
                    Source = "zapalka.png",
                    WidthRequest = 30,
                    HeightRequest = 100
                };
                imageContainer.Children.Add(zapalka);
            }
            liczbaZapalekLbl.Text = "Pozostało zapałek: " + liczbaZapalek;
            logsScrollView.Children.Add(label);
           
        }

        private bool CheckGameOver(string winner)
        {
            if (liczbaZapalek == 0)
            {
                DisplayAlert("Koniec gry", $"{winner} wygrał!", "OK");
                return true;
            }
            return false;
        }
    }
}
