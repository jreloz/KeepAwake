using System;
using System.Threading;
using WindowsInput;
using System.Windows;

namespace KeepAwake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 




    public partial class MainWindow
    {

        InputSimulator inputSimulator = new InputSimulator();


        string selection = "";
        bool keepawake = false;

        const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
        const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;

        // We need to use unmanaged code
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, uint dwExtraInf);



       

        public MainWindow()
        {
            InitializeComponent();
        }


       
        private void MouseLeftClick()
        {

            Point defPnt = new Point();
            GetCursorPos(ref defPnt);

            do
            {

                this.Dispatcher.Invoke(() =>
                {
                    TxtKeepWake.Text += "Mouse Click\n";
                });


                mouse_event(MOUSEEVENTF_LEFTDOWN, Convert.ToUInt32(defPnt.X), Convert.ToUInt32(defPnt.Y), 0, 0);//make left button down
                Thread.Sleep(1000);
                mouse_event(MOUSEEVENTF_LEFTUP, Convert.ToUInt32(defPnt.X), Convert.ToUInt32(defPnt.Y), 0, 0);//make left button up


                if (keepawake == false)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        BtnMouseLeftClick.Content = "Mouse Click";
                        TxtKeepWake.Text += "Stopped!\n";
                    });

                    break;
                }
                Thread.Sleep(2000);

            } while (keepawake == true);
           
        }








        private void WriteSomething()
        {

            do
            {
                this.Dispatcher.Invoke(() =>
                {
                    TxtKeepWake.Text += "Writing...\n";
                });


                // inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                inputSimulator.Keyboard.TextEntry("Be right back! \n");
                Thread.Sleep(1000);


                if (keepawake == false)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        BtnWriteText.Content = "Write Something";
                        TxtKeepWake.Text += "Stopped!\n";
                    });

                    break;
                }
                Thread.Sleep(2000);

            } while (keepawake == true);  
        }








        


        private void BtnMouseLeftClick_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (keepawake == true)
            {
                keepawake = false;
                BtnMouseLeftClick.Content = "Mouse Click";
            }
            else
            {
                selection = "mouse_left_click";
                Thread thread = new Thread(new ThreadStart(Start));
                thread.Start();

            }
            
        }










        private void BtnWriteText_Click(object sender, System.Windows.RoutedEventArgs e)
        {
          
            if (keepawake == true)
            {
                keepawake = false;
                BtnWriteText.Content = "Write Text";
            }
            else
            {
                selection = "write_text";
                Thread thread = new Thread(new ThreadStart(Start));
                thread.Start();

            }

        }



        








        private void Start()
        {

            Thread thread;

            for (int i = 5; i > 0; i--)
            {

                this.Dispatcher.Invoke(() =>
                {

                    switch (selection)
                    {
                        case "mouse_left_click":
                            BtnMouseLeftClick.Content = $"{i}";
                            break;

                        case "write_text":
                            BtnWriteText.Content = $"{i}";
                            break;
                        default:
                            break;
                    }

                });

                Thread.Sleep(1000);
            }






            switch (selection)
            {
                case "mouse_left_click":

                    keepawake = true;
                    this.Dispatcher.Invoke(() =>
                    {
                        BtnMouseLeftClick.Content = "Stop";
                    });
                   
                    thread = new Thread(new ThreadStart(MouseLeftClick));
                    thread.Start();

                    break;

                case "write_text":

                    keepawake = true;
                    this.Dispatcher.Invoke(() =>
                    {
                        BtnWriteText.Content = "Stop";
                    });
                    
                    thread = new Thread(new ThreadStart(WriteSomething));
                    thread.Start();

                    break;

                default:
                    break;
            }
        }











        




    }
}
