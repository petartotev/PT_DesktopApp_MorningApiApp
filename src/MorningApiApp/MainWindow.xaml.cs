using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using MorningApiApp.ExternalServices.NewsApiOrg;
using MorningApiApp.ExternalServices.NewsApiOrg.Enums;
using MorningApiApp.ExternalServices.NewsApiOrg.Mappers;
using MorningApiApp.ExternalServices.NewsApiOrg.Models;
using MorningApiApp.ExternalServices.NewsApiOrg.OutputModels;
using MorningApiApp.ExternalServices.OpenWeatherApi;
using MorningApiApp.ExternalServices.OpenWeatherApi.Enums;
using MorningApiApp.ExternalServices.OpenWeatherApi.Mappers;
using MorningApiApp.ExternalServices.OpenWeatherApi.Models;
using MorningApiApp.ExternalServices.OpenWeatherApi.OutputModels;

namespace MorningApiApp
{
    public partial class MainWindow : Window
    {
        private readonly OpenWeatherApiHttpClient _openWeatherApiClient = new OpenWeatherApiHttpClient();
        private readonly NewsApiOrgHttpClient _newsApiOrgHttpClient = new NewsApiOrgHttpClient();

        // News filter
        private ComboBox _comboBoxNewsEndpoint;
        private Label _labelNewsFrom;
        private Calendar _calendarNewsFrom;
        private Label _labelNewsTo;
        private Calendar _calendarNewsTo;
        private Label _labelNewsSource;
        private ComboBox _comboBoxNewsSource;
        private Label _labelNewsTextInput;
        private TextBox _textBoxTextInput;
        private Label _labelNewsCategory;
        private ComboBox _comboBoxNewsCategory;
        private Label _labelNewsOr;
        private Label _labelNewsAnd;
        private Label _labelNewsCountry;
        private ComboBox _comboBoxNewsCountry;
        private Label _labelNewsSort;
        private ComboBox _comboBoxNewsSort;
        private Button _buttonNewsGo;

        // News Article
        private List<MyNewsArticle> _listArticlesSelected;
        private DockPanel _dockPanelArticle = new DockPanel();
        private Slider _sliderArticles = new Slider();

        public MainWindow()
        {
            InitializeComponent();

            CreateMenuOnGrid();
        }

        // ========== Create Elements (on Grid) ==========
        // ===== Menu =====
        private void CreateMenuOnGrid()
        {
            Menu myMenu = new Menu
            {
                Name = "MyMenu",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top
            };
            _ = MyGrid.Children.Add(myMenu);

            MenuItem fileMenuItem = new MenuItem
            {
                Header = "_File"
            };
            myMenu.Items.Add(fileMenuItem);

            CreateMenuItemInMenuItem(fileMenuItem, "_Open", ClickOnMenuButtonOpen, new ToolTip { Content = "Click to open..." });
            CreateMenuItemInMenuItem(fileMenuItem, "_News", ClickOnMenuButtonNews, new ToolTip { Content = "Click to see news..." });
            CreateMenuItemInMenuItem(fileMenuItem, "_Weather", ClickOnMenuButtonWeather, new ToolTip { Content = "Click to see weather..." });
            CreateMenuItemInMenuItem(fileMenuItem, "_Exit", ClickOnMenuButtonExit, new ToolTip { Content = "Click to exit..." });
        }

        private void CreateMenuItemInMenuItem(MenuItem menu, string header, RoutedEventHandler handler, ToolTip toolTip = null)
        {
            MenuItem newMenuItem = new MenuItem
            {
                Header = header,
                ToolTip = toolTip ?? new ToolTip { Content = header }
            };
            newMenuItem.Click += handler;
            _ = menu.Items.Add(newMenuItem);
        }

        // ===== Main =====
        private TextBox CreateTextBoxOnGrid(
            string text,
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment verticalAlignment = VerticalAlignment.Center,
            Action<TextBox> setup = null)
        {
            TextBox myTextBox = new TextBox
            {
                Text = text,
                HorizontalAlignment = horizontalAlignment,
                VerticalAlignment = verticalAlignment,
            };

            setup?.Invoke(myTextBox);

            _ = MyGrid.Children.Add(myTextBox);

            return myTextBox;
        }

        private Label CreateLabelOnGrid(
            string content,
            Thickness margin,
            Action<Label> setup = null)
        {
            Label label = new Label
            {
                Width = 200,
                Height = 30,
                Background = new SolidColorBrush(Colors.White),
                Foreground = new SolidColorBrush(Colors.Black),
                Content = content,
                Margin = margin
            };

            setup?.Invoke(label);

            _ = MyGrid.Children.Add(label);

            return label;
        }

        private Calendar CreateCalendarOnGrid(DateTime selectedDate, Thickness thickness)
        {
            Calendar myCalendar = new Calendar
            {
                SelectedDate = selectedDate,
                Margin = thickness
            };

            _ = MyGrid.Children.Add(myCalendar);

            return myCalendar;
        }

        private ComboBox CreateComboBox(
            int width,
            int height,
            Thickness margin,
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center,
            Action<ComboBox> action = null)
        {
            ComboBox comboBox = new ComboBox
            {
                Width = width,
                Height = height,
                HorizontalAlignment = horizontalAlignment,
                Margin = margin
            };

            action?.Invoke(comboBox);

            return comboBox;
        }

        private Image CreateImageWithUrl(string name, string url, Action<Image> setup = null)
        {
            var thing = Environment.CurrentDirectory;
            Uri uri = new Uri(@$"../../../default.jpg", UriKind.Relative);

            if (!string.IsNullOrWhiteSpace(url) && url.StartsWith("http"))
            {
                uri = new Uri(url, UriKind.RelativeOrAbsolute);
            }

            BitmapImage imageBitmap = new BitmapImage();

            imageBitmap.BeginInit();
            imageBitmap.UriSource = uri;
            imageBitmap.EndInit();

            Image image = new Image
            {
                Name = name,
                RenderTransformOrigin = new Point(0.5, 1),
                Source = imageBitmap
            };

            setup?.Invoke(image);

            return image;
        }

        // ===== News =====
        private ComboBox CreateComboBoxWeatherCitiesOnGrid()
        {
            ComboBox myComboBox = CreateComboBox(200, 30, new Thickness(0, -480, 0, 0), HorizontalAlignment.Center);

            foreach (var city in Enum.GetValues(typeof(WeatherCityEnum)))
            {
                myComboBox.Items.Add(new ComboBoxItem { Content = (WeatherCityEnum)city });
            }

            _ = MyGrid.Children.Add(myComboBox);

            return myComboBox;
        }

        private ComboBox CreateComboBoxNewsEndpointsOnGrid()
        {
            ComboBox comboBoxNewsEndpoints = CreateComboBox(240, 24, new Thickness(0, -600, 0, 0), HorizontalAlignment.Center);

            foreach (var category in Enum.GetValues(typeof(NewsEndpointEnum)))
            {
                comboBoxNewsEndpoints.Items.Add(new ComboBoxItem { Content = (NewsEndpointEnum)category });
            }

            _ = MyGrid.Children.Add(comboBoxNewsEndpoints);

            return comboBoxNewsEndpoints;
        }

        private ComboBox CreateComboBoxNewsSourcesOnGrid()
        {
            _comboBoxNewsSource = CreateComboBox(240, 24, new Thickness(0, 90, 0, 0), HorizontalAlignment.Center, x => x.FontSize = 12);

            foreach (var source in Enum.GetValues(typeof(NewsSourceEnum)))
            {
                _comboBoxNewsSource.Items.Add(new ComboBoxItem { Content = (NewsSourceEnum)source });
            }

            _ = MyGrid.Children.Add(_comboBoxNewsSource);

            return _comboBoxNewsSource;
        }

        private ComboBox CreateComboBoxNewsCategoryOnGrid()
        {
            _comboBoxNewsCategory = CreateComboBox(240, 24, new Thickness(-260, -100, 0, 0), HorizontalAlignment.Center, x => x.FontSize = 12);

            foreach (var category in Enum.GetValues(typeof(NewsCategoryEnum)))
            {
                _comboBoxNewsCategory.Items.Add(new ComboBoxItem { Content = (NewsCategoryEnum)category });
            }

            _ = MyGrid.Children.Add(_comboBoxNewsCategory);

            return _comboBoxNewsCategory;
        }

        private ComboBox CreateComboBoxNewsCounryOnGrid()
        {
            _comboBoxNewsCountry = CreateComboBox(240, 24, new Thickness(260, -100, 0, 0), HorizontalAlignment.Center, x => x.FontSize = 12);

            foreach (var country in Enum.GetValues(typeof(NewsCountryEnum)))
            {
                _comboBoxNewsCountry.Items.Add(new ComboBoxItem { Content = (NewsCountryEnum)country });
            }

            _ = MyGrid.Children.Add(_comboBoxNewsCountry);

            return _comboBoxNewsCountry;
        }

        private ComboBox CreateComboBoxNewsSortByOnGrid()
        {
            _comboBoxNewsSort = CreateComboBox(240, 24, new Thickness(0, 240, 0, 0), HorizontalAlignment.Center, x => x.FontSize = 12);

            foreach (var sort in Enum.GetValues(typeof(NewsSortEnum)))
            {
                _comboBoxNewsSort.Items.Add(new ComboBoxItem { Content = (NewsSortEnum)sort });
            }

            _ = MyGrid.Children.Add(_comboBoxNewsSort);

            return _comboBoxNewsSort;
        }

        // ===== Menu Buttons Click Events =====
        private void ClickOnMenuButtonOpen(object sender, RoutedEventArgs e)
        {
            RemoveAllGridElementsButMenu();
        }

        private void ClickOnMenuButtonWeather(object sender, RoutedEventArgs e)
        {
            RemoveAllGridElementsButMenu();

            ComboBox comboBoxWeather = CreateComboBoxWeatherCitiesOnGrid();
            comboBoxWeather.SelectionChanged += ClickOnComboBoxWeather;
        }

        private void ClickOnMenuButtonNews(object sender, RoutedEventArgs e)
        {
            RemoveAllGridElementsButMenu();

            _comboBoxNewsEndpoint = CreateComboBoxNewsEndpointsOnGrid();
            _comboBoxNewsEndpoint.SelectionChanged += ClickOnComboBoxNewsEndpoints;
        }

        private void ClickOnMenuButtonExit(object sender, RoutedEventArgs e)
        {
            RemoveAllGridElementsButMenu();

            Application.Current.Shutdown();
        }

        // ===== Other Click Events =====
        private void ClickOnComboBoxWeather(object sender, RoutedEventArgs e)
        {
            ComboBox comboBoxSending = (ComboBox)sender;
            ComboBoxItem comboBoxSelectedItem = (ComboBoxItem)comboBoxSending.SelectedItem;

            string responseString = _openWeatherApiClient.GetHttpResponse((WeatherCityEnum)comboBoxSelectedItem.Content);

            if (!string.IsNullOrEmpty(responseString))
            {
                RootWeather rootWeatherApiModel = JsonConvert.DeserializeObject<RootWeather>(responseString);
                MyRootWeatherModel outputModel = rootWeatherApiModel.ToMyRootWeatherModel();

                _ = CreateTextBoxOnGrid(outputModel.ToString(), setup: x =>
                {
                    x.Height = x.Width = 200;
                    x.Foreground = new SolidColorBrush(outputModel.GetColorByTemperature().ForegroundColor);
                    x.Background = new SolidColorBrush(outputModel.GetColorByTemperature().BackgroundColor);
                });
            }
        }

        private void ClickOnComboBoxNewsEndpoints(object sender, RoutedEventArgs e)
        {
            ComboBox comboBoxSender = (ComboBox)sender;
            ComboBoxItem comboBoxSelectedItem = (ComboBoxItem)comboBoxSender.SelectedItem;

            _labelNewsFrom = CreateLabelOnGrid("From", new Thickness(-200, -530, 0, 0));
            _calendarNewsFrom = CreateCalendarOnGrid(DateTime.Now, new Thickness(-200, 80, 0, 0));

            _labelNewsTo = CreateLabelOnGrid("To", new Thickness(200, -530, 0, 0));
            _calendarNewsTo = CreateCalendarOnGrid(DateTime.Now, new Thickness(200, 80, 0, 0));

            _labelNewsSource = CreateLabelOnGrid("Source", new Thickness(-50, 50, 0, 0));
            _comboBoxNewsSource = CreateComboBoxNewsSourcesOnGrid();
            _comboBoxNewsSource.SelectionChanged += ClearNewsCategoryAndCountry;

            _labelNewsSort = CreateLabelOnGrid("Sort By", new Thickness(-50, 200, 0, 0));
            _comboBoxNewsSort = CreateComboBoxNewsSortByOnGrid();

            if (comboBoxSelectedItem.Content.ToString() == NewsEndpointEnum.Everything.ToString())
            {
                MyGrid.Children.Remove(_labelNewsCategory);
                MyGrid.Children.Remove(_comboBoxNewsCategory);

                MyGrid.Children.Remove(_labelNewsCountry);
                MyGrid.Children.Remove(_comboBoxNewsCountry);

                MyGrid.Children.Remove(_labelNewsOr);
                MyGrid.Children.Remove(_labelNewsAnd);

                _labelNewsTextInput = CreateLabelOnGrid("Enter keyword...", new Thickness(-195, -140, 0, 0));
                _textBoxTextInput = CreateTextBoxOnGrid("Donuts", setup: x =>
                {
                    x.Width = 380;
                    x.Height = 25;
                    x.Margin = new Thickness(0, -100, 0, 0);
                });

                _labelNewsAnd = CreateLabelOnGrid("and", new Thickness(5, 0, 0, 0), x =>
                {
                    x.HorizontalAlignment = HorizontalAlignment.Center;
                    x.Width = 30;
                });
            }
            else // Top-Headlines
            {
                MyGrid.Children.Remove(_labelNewsTextInput);
                MyGrid.Children.Remove(_textBoxTextInput);

                MyGrid.Children.Remove(_labelNewsOr);
                MyGrid.Children.Remove(_labelNewsAnd);

                _labelNewsCategory = CreateLabelOnGrid("Category", new Thickness(-300, -140, 0, 0));
                _comboBoxNewsCategory = CreateComboBoxNewsCategoryOnGrid();
                _comboBoxNewsCategory.SelectionChanged += ClearNewsSources;

                _labelNewsCountry = CreateLabelOnGrid("Country", new Thickness(220, -140, 0, 0));
                _comboBoxNewsCountry = CreateComboBoxNewsCounryOnGrid();
                _comboBoxNewsCountry.SelectionChanged += ClearNewsSources;

                _labelNewsOr = CreateLabelOnGrid("or", new Thickness(5, 0, 0, 0), x =>
                {
                    x.HorizontalAlignment = HorizontalAlignment.Center;
                    x.Width = 30;
                });
            }

            _buttonNewsGo = new Button
            {
                Content = "GO",
                Width = 240,
                Height = 120,
                Background = new SolidColorBrush(Colors.Magenta),
                Foreground = new SolidColorBrush(Colors.Black),
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 480, 0, 0),
                FontSize = 32

            };
            _buttonNewsGo.Click += ClickOnNewsGoButton;
            _ = MyGrid.Children.Add(_buttonNewsGo);
        }

        private void ClickOnNewsGoButton(object sender, RoutedEventArgs e)
        {
            RemoveAllGridElementsButMenu();

            string url = _newsApiOrgHttpClient.ComposeUrlByInput(
                    _comboBoxNewsEndpoint == null ? null : ((ComboBoxItem)_comboBoxNewsEndpoint.SelectedItem)?.Content.ToString(),
                    _calendarNewsFrom?.SelectedDate,
                    _calendarNewsTo?.SelectedDate,
                    _comboBoxNewsSort == null ? null : ((ComboBoxItem)_comboBoxNewsSort.SelectedItem)?.Content.ToString(),
                    _comboBoxNewsSource == null ? null : ((ComboBoxItem)_comboBoxNewsSource.SelectedItem)?.Content.ToString(),
                    _textBoxTextInput?.Text,
                    _comboBoxNewsCategory == null ? null : ((ComboBoxItem)_comboBoxNewsCategory.SelectedItem)?.Content.ToString(),
                    _comboBoxNewsCountry == null ? null : ((ComboBoxItem)_comboBoxNewsCountry.SelectedItem)?.Content.ToString());

            string responseString = _newsApiOrgHttpClient.GetHttpResponse(url);

            if (!string.IsNullOrEmpty(responseString))
            {
                RootNews rootNews = JsonConvert.DeserializeObject<RootNews>(responseString);
                MyRootNews outputModel = rootNews.ToMyRootNewsModel();

                _listArticlesSelected = outputModel.MyArticles;

                if (_listArticlesSelected != null && _listArticlesSelected.Count >= 1)
                {
                    Label labelArticlesCount = new Label
                    {
                        Width = 120,
                        Height = 30,
                        Content = $"{_listArticlesSelected.Count} articles found.",
                        Background = new SolidColorBrush(Colors.Magenta),
                        Foreground = new SolidColorBrush(Colors.Black),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Margin = new Thickness(0, 600, 0, 0)
                    };
                    MyGrid.Children.Add(labelArticlesCount);

                    Button buttonBack = new Button
                    {
                        Width = 90,
                        Height = 30,
                        Content = $"BACK",
                        Background = new SolidColorBrush(Colors.Cyan),
                        Foreground = new SolidColorBrush(Colors.Black),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(0, -600, 0, 0)
                    };
                    buttonBack.Click += ClickOnMenuButtonNews;
                    MyGrid.Children.Add(buttonBack);

                    _sliderArticles = new Slider
                    {
                        Width = 900,
                        Height = 20,
                        Maximum = _listArticlesSelected.Count - 1,
                        Value = 0,
                        IsSnapToTickEnabled = true,
                        TickFrequency = 1,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(0, -600, 0, 0),
                    };
                    _sliderArticles.ValueChanged += ChangeValueOfSlider;
                    MyGrid.Children.Add(_sliderArticles);

                    Button buttonLeft = new Button
                    {
                        Content = "☚",
                        Width = 30,
                        Height = 30,
                        FontSize = 15,
                        Margin = new Thickness(-950, -600, 0, 0),
                    };
                    buttonLeft.Click += ClickOnButtonArticlesLeft;
                    MyGrid.Children.Add(buttonLeft);

                    Button buttonRight = new Button
                    {
                        Content = "☛",
                        Width = 30,
                        Height = 30,
                        FontSize = 15,
                        Margin = new Thickness(950, -600, 0, 0),
                    };
                    buttonRight.Click += ClickOnButtonArticlesRight;
                    MyGrid.Children.Add(buttonRight);

                    if (_listArticlesSelected != null && _listArticlesSelected.Count > 0)
                    {
                        VisualizeCurrentArticle(_listArticlesSelected[0]);
                    }
                }
            }
        }

        private void ClickOnButtonArticlesLeft(object sender, RoutedEventArgs e)
        {
            if (_sliderArticles != null && _sliderArticles.Value > 0)
            {
                _sliderArticles.Value -= 1;
            }
        }

        private void ClickOnButtonArticlesRight(object sender, RoutedEventArgs e)
        {
            if (_sliderArticles != null && _sliderArticles.Value < _listArticlesSelected.Count - 1)
            {
                _sliderArticles.Value += 1;
            }
        }

        private void ChangeValueOfSlider(object sender, RoutedEventArgs e)
        {
            MyGrid.Children.Remove(_dockPanelArticle);

            Slider sliderSender = (Slider)sender;
            int sliderSenderValue = Convert.ToInt32(sliderSender.Value);
            MyNewsArticle article = _listArticlesSelected[sliderSenderValue];

            VisualizeCurrentArticle(article);
        }

        private void VisualizeCurrentArticle(MyNewsArticle article)
        {
            _dockPanelArticle = new DockPanel
            {
                Width = 1200,
                Height = 550
            };
            MyGrid.Children.Add(_dockPanelArticle);

            // Title Border = 30
            Border borderTitle = new Border
            {
                Height = 30,
            };
            borderTitle.Child = new TextBlock
            {
                Text = article.Title,
                FontSize = 21,
                FontWeight = FontWeights.Bold,
                TextWrapping = TextWrapping.Wrap
            };
            borderTitle.SetValue(DockPanel.DockProperty, Dock.Top);
            _ = _dockPanelArticle.Children.Add(borderTitle);

            // Description Border = 50
            Border borderDescription = new Border
            {
                Height = 50,
                Width = 1200
            };
            borderDescription.Child = new TextBlock
            {
                Text = article.Description?.Replace(". ", ".\n"),
                FontSize = 18,
                TextWrapping = TextWrapping.Wrap
            };
            borderDescription.SetValue(DockPanel.DockProperty, Dock.Top);
            _ = _dockPanelArticle.Children.Add(borderDescription);

            // Author and Date Border = 20
            Border borderAuthorAndDate = new Border
            {
                Height = 20,
            };
            borderAuthorAndDate.Child = new TextBlock
            {
                Text = $" by {article.Author} / ({article.PublishedAt.ToString("yyyy-MM-dd-HH-mm-ss")})",
                FontSize = 12,
                TextWrapping = TextWrapping.Wrap
            };
            borderAuthorAndDate.SetValue(DockPanel.DockProperty, Dock.Top);
            _ = _dockPanelArticle.Children.Add(borderAuthorAndDate);

            // Link Border = 25
            Border borderUrl = new Border
            {
                Height = 25,
            };
            var textBlockUrl = new TextBlock
            {
                FontSize = 15,
                TextWrapping = TextWrapping.Wrap
            };
            Hyperlink hyperLinkUrl = new Hyperlink();
            hyperLinkUrl.Inlines.Add(article.Url.ToString());
            hyperLinkUrl.NavigateUri = new Uri(article.Url.ToString());
            hyperLinkUrl.IsEnabled = true;
            hyperLinkUrl.RequestNavigate += RedirectToArticleUrlAddress;
            textBlockUrl.Inlines.Add(hyperLinkUrl);
            borderUrl.Child = textBlockUrl;
            borderUrl.SetValue(DockPanel.DockProperty, Dock.Bottom);
            _ = _dockPanelArticle.Children.Add(borderUrl);

            // Image Border = 425 x 600
            Border borderImage = new Border
            {
                Height = 425,
                Width = 600,
                Padding = new Thickness(10, 10, 10, 10)
            };
            borderImage.Child = CreateImageWithUrl("ArticleImage", article.UrlToImage);
            borderImage.SetValue(DockPanel.DockProperty, Dock.Left);
            _ = _dockPanelArticle.Children.Add(borderImage);

            // Content Border = 425 x 600
            Border borderContent = new Border
            {
                Height = 425,
                Width = 600,
                Padding = new Thickness(25, 25, 25, 25)
            };
            borderContent.Child = new TextBlock
            {
                Text = article.Content,
                FontSize = 15,
                TextWrapping = TextWrapping.Wrap
            };
            borderContent.SetValue(DockPanel.DockProperty, Dock.Right);
            _ = _dockPanelArticle.Children.Add(borderContent);
        }

        private void RedirectToArticleUrlAddress(object sender, RequestNavigateEventArgs e)
        {
            var processRedirect = new ProcessStartInfo(e.Uri.AbsoluteUri)
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(processRedirect);
            e.Handled = true;
        }

        // ===== Clear and Remove =====
        private void ClearNewsCategoryAndCountry(object sender, SelectionChangedEventArgs e)
        {
            if (_comboBoxNewsCategory != null)
            {
                _comboBoxNewsCategory.SelectedIndex = -1;

            }
            if (_comboBoxNewsCountry != null)
            {
                _comboBoxNewsCountry.SelectedIndex = -1;
            }
        }

        private void ClearNewsSources(object sender, SelectionChangedEventArgs e)
        {
            if (_comboBoxNewsSource != null)
            {
                _comboBoxNewsSource.SelectedIndex = -1;
            }
        }

        private void RemoveAllGridElementsButMenu()
        {
            MyGrid.Children.RemoveRange(1, MyGrid.Children.Count - 1);
        }
    }
}
