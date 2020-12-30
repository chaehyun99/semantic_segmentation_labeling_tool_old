using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gray2RGB_in_test_JSW
{
    public partial class Form1 : Form
    {
        Image mOriginal;

        public Form1()
        {
            InitializeComponent();

            
        }

        //이미지 로드
        private void Image_Load_Click(object sender, EventArgs e)
        {
            //창 띄워서 파일 불러오기
            //pictureBox1.Load(@"C:\Users\KHS\Desktop\현수\증명사진_권현수.jpg");
            
            string image_file = string.Empty;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = @"C:\";
            var dialogResult = dialog.ShowDialog();

            if(dialogResult == DialogResult.OK)
            {
                image_file = dialog.FileName;
            }
            else if(dialogResult == DialogResult.Cancel)
            {
                return;
            }
            
            pictureBox1.Image = Bitmap.FromFile(image_file);
            mOriginal = pictureBox1.Image;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        //이미지 저장

        public static FolderBrowserDialog GetSaveFolderDialog()
        {
            FolderBrowserDialog saveFolderDig = new FolderBrowserDialog();
            saveFolderDig.RootFolder = Environment.SpecialFolder.Desktop;

            return saveFolderDig;
        }

        private void Image_Save_Click(Object sender, EventArgs e)
        {
            using(FolderBrowserDialog folderDiag = GetSaveFolderDialog())
            {
                if(folderDiag.ShowDialog(this) == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;

                    string dir = folderDiag.SelectedPath;

                    Image image = pictureBox1.Image;

                    string fileName = "test.jpg".ToString();

                    if(image != null)
                    {
                        string imageSavePath = string.Format(@"{0}\{1}", dir, fileName);
                        image.Save(imageSavePath);
                    }

                    MessageBox.Show("저장 완료");

                    this.Cursor = Cursors.Default;

                }

            }

        }

        public Color Grey2RGB(Color co)
        {
            int a = co.A;
            int r = co.R;
            int g = co.G;
            int b = co.B;
            

            //grey를 rgb로 어떻게 수정할 것인지 if문으로 하나하나 정하기
            a = 255;
            r = r ^ 255;
            g = g ^ 255;
            b = b ^ 255;
            

            co = Color.FromArgb(a, r, g, b);
            return co;
        }

        private void RGB(Object sender, EventArgs e)
        {
            Color co;
            Color ret_co;

            Bitmap temp = mOriginal as Bitmap;
            

            for (int x = 0; x < temp.Width; x++)
            {
                for (int y = 0; y < temp.Height; y++)
                {
                    co = temp.GetPixel(x, y);

                    ret_co = Grey2RGB(co);


                    temp.SetPixel(x, y, ret_co);     // 해당 좌표 픽셀의 컬러값을 변경
                }
            }
            pictureBox1.Image = temp;

            MessageBox.Show("색상 반전 완료");
        }


        public Color Grey2RGB_Ver2(Color co)
        {
            byte Gray_Value = co.R;  // 여기서 받는 값은 grayscale 이미지의 픽셀에서 값 (0~20)이 들어와야 됨.
                                     //byte in c# 은 uint8_t in c 에 대응함(아마도)
                                     //현재 png-8 포맷을 비트맵으로 가져왔을때 값이 어떻게 될지를 모르므로 나중에 확인해서 수정.

            //이하 컬러맵 인덱스별 대응값.(in python 네트워크 모델)
            /* COLORS = ([
                 (0, 0, 0),       // 0=background
                 (128, 0, 0),     # 1=aeroplane
                 (0, 128, 0),     # 2=bicycle
                 (128, 128, 0),   # 3=bird
                 (0, 0, 128),     # 4=boat
                 (128, 0, 128),   # 5=bottle
                 (0, 128, 128),   # 6=bus
                 (128, 128, 128), # 7=car
                 (255, 255, 255), # 8=cat
                 (192, 0, 0),     # 9=chair
                 (64, 128, 0),    # 10=cow
                 (192, 128, 0),   # 11=dining table
                 (64, 0, 128),    # 12=dog
                 (192, 0, 128),   # 13=horse
                 (64, 128, 128),  # 14=motorbike
                 (192, 128, 128), # 15=person
                 (0, 64, 0),      # 16=potted plant
                 (128, 64, 0),    # 17=sheep
                 (0, 192, 0),     # 18=sofa
                 (128, 192, 0),   # 19=train
                 (0, 64, 128)     # 20=tv/monitor
             ])

            */
            //c#에 맞게 사용. 색상 셋은 r언어나 matlab에서 쓰는거중에 시인성 좋은걸로.

            Color[] ColorTable = new Color[]
            {
                Color.Black ,         // 0=background
                Color.LightPink,        // 1=aeroplane
                Color.DarkGreen,       //2=bicycle
                Color.LightBlue,        // 3=bird
                Color.LawnGreen,        // 4=boat
                Color.Lavender,         // 5=bottle
                Color.Khaki,            // 6=bus
                Color.Ivory,            // 7=car
                Color.IndianRed,        // 8=cat
                Color.HotPink,          // 9=chair
                Color.Lime,             // 10=cow
                Color.LightCoral,    // 11=dining table
                Color.LightSalmon,      // 12=dog
                Color.SlateBlue,        // 13=horse
                Color.OrangeRed,        // 14=motorbike
                Color.Yellow,           // 15=person
                Color.Navy,             // 16=potted plant
                Color.Wheat,            // 17=sheep
                Color.MediumTurquoise,  // 18=sofa
                Color.Magenta,          // 19=train
                Color.Gray,             // 20=tv/monitor
                //이하 테스트 전용 색상들임.
                Color.Tan,
                Color.Aqua,
                Color.DarkCyan,
                Color.DarkKhaki,
                Color.LemonChiffon,
                Color.DeepPink,
                Color.Pink,
                Color.LightCoral,
                Color.AntiqueWhite,
                Color.AliceBlue, //30        
                Color.DarkCyan,
                Color.BlueViolet,
                Color.Chartreuse,
                Color.DarkOliveGreen,
                Color.Cornsilk,
                Color.DarkOrange,
                Color.Gainsboro,
                Color.Blue,
                Color.Bisque
            };

            co = ColorTable[Gray_Value];
            return co;
        }

        private void RGB_Ver2(Object sender, EventArgs e)
        {
            //기존에 현수가 짜놓은 grayscale2rgb에서 클래스 테이블에 맞게 변형.

            Bitmap image1 = mOriginal as Bitmap;
            //mOriginal : 
            //1.폼 코드 제일 앞에서 Image mOriginal 정의.
            //2.이미지 로딩하는 버튼 클릭이벤트 마지막쯤에 다음 코드 삽입.
            //mOriginal = pictureBox1.Image;

            int x, y;

            //사용하는 데이터셋에 따라 클래스의 인덱스값이 다르지만
            //어짜피 값하나당 색깔하나라는건 변하지 않으므로 상관없다.

            // Loop through the images pixels to reset color.
            for (x = 0; x < image1.Width; x++)
            {
                for (y = 0; y < image1.Height; y++)
                {
                    Color pixelColor = image1.GetPixel(x, y);
                    if (pixelColor.R > 30)
                    {
                        string str_test = Convert.ToString(x) + "&" + Convert.ToString(y);
                        string str_rgb = Convert.ToString(pixelColor.R)
                            + "," + Convert.ToString(pixelColor.G)
                            + "," + Convert.ToString(pixelColor.B) + "\n";
                        MessageBox.Show(str_rgb + str_test);
                    }

                    Color newColor = Grey2RGB_Ver2(pixelColor);
                    image1.SetPixel(x, y, newColor);
                }
            }

            pictureBox1.Image = image1;
            //pictur_Box: 변환 완료된 RGB이미지를 띄울 픽쳐박스명.


        }

    
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        List<string> imgList = null;

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                UiTxt_Folder.Text = fbd.SelectedPath;
            }

            this.uiFp_Image.Controls.Clear();

            string[] files = Directory.GetFiles(fbd.SelectedPath);
            imgList = files.Where(x => x.IndexOf(".jpg", StringComparison.OrdinalIgnoreCase) >= 0 || x.IndexOf(".png", StringComparison.OrdinalIgnoreCase) >= 0).Select(x => x).ToList();

            for (int i = 0; i< imgList.Count; i++)
            {
                Image img = Image.FromFile(imgList[i]);

                Panel pPanel = new Panel();
                pPanel.BackColor = Color.Black;
                pPanel.Size = new Size(150, 150);
                pPanel.Padding = new System.Windows.Forms.Padding(4);

                PictureBox pBox = new PictureBox();
                pBox.BackColor = Color.DimGray;
                pBox.Dock = DockStyle.Fill;
                pBox.SizeMode = PictureBoxSizeMode.Zoom;
                pBox.Image = img.GetThumbnailImage(150, 150, null, IntPtr.Zero);
                pBox.Click += PBox_Click;
                pBox.Tag = i.ToString();
                pPanel.Controls.Add(pBox);

                this.uiFp_Image.Controls.Add(pPanel);
            }

            if(imgList.Count > 0)
            {
                Panel pnl = this.uiFp_Image.Controls[0] as Panel;
                PictureBox pb = pnl.Controls[0] as PictureBox;
                UiTxt_File.Text = this.imgList[0];
                PBox_Click(pb, null);
            }
            
            
        }

        public void PBox_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.uiFp_Image.Controls.Count; i++)
            {
                if(this.uiFp_Image.Controls[i] is Panel)
                {
                    Panel pnl = this.uiFp_Image.Controls[i] as Panel;
                    pnl.BackColor = Color.Black;
                }
            }

            PictureBox pb = sender as PictureBox;
            pb.Parent.BackColor = Color.Red;

            int idx = Convert.ToInt32(pb.Tag.ToString());

            Image img = Image.FromFile(imgList[idx]);
            pictureBox1.Image = img;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            UiTxt_File.Text = this.imgList[idx];
        }

        private void uiFp_Image_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void uiFp_Image_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }

}
