using DevExpress.Skins;
using DevExpress.Skins.XtraForm;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP
{
    public partial class NewSenarioForm : DevExpress.XtraEditors.XtraForm
    {
        string selectedFuselage = string.Empty;

        public NewSenarioForm()
        {
            InitializeComponent();
        }

        // Form Title 을 중앙에 배치
        protected override DevExpress.Skins.XtraForm.FormPainter CreateFormBorderPainter()
        {
            HorzAlignment formCaptionAlignment = HorzAlignment.Center;
            return new CustomFormPainter(this, LookAndFeel, formCaptionAlignment);
        }

        TextEdit[] txtList;
        string strInputScenarioName = Properties.Resources.InputScenarioName;
        string strAddTag = Properties.Resources.AddTag;

        private void NewSenarioForm_Load(object sender, EventArgs e)
        {
            InitializeScenarioTypeList();
            InitializeFuselageList();

            //시나리오 이름, 태그 TextEdit Placeholder 설정
            txtList = new TextEdit[] { edtScenarioName, edtTag };
            foreach (var txt in txtList)
            {
                //처음 공백 Placeholder 지정
                txt.ForeColor = Color.DarkGray;
                if (txt == edtScenarioName) txt.Text = strInputScenarioName;
                else if (txt == edtTag) txt.Text = strAddTag;

                txt.Focus();

                //텍스트박스 커서 Focus 여부에 따라 이벤트 지정
                txt.GotFocus += RemovePlaceholder;
                txt.LostFocus += SetPlaceholder;
            }

            DateTime dtNow = DateTime.Now;
            string strNow = string.Format("{0:yyyy-MM-dd}", dtNow);
            dateScenario.Text = strNow;


            //// for testing CheckEdit
            checkEdit1.Visible = false;
            checkEdit1.BorderStyle = BorderStyles.Simple;
            checkEdit1.RightToLeft = RightToLeft.Yes;
            checkEdit1.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgToggle1;
            checkEdit1.Properties.Appearance.BorderColor = Color.Gray;
            checkEdit1.CheckedChanged += checkEdit1_CheckedChanged;
            //// for testing CheckEdit

            //// for testing ButtonEdit
            buttonEdit1.Visible = false;
            buttonEdit1.Properties.Buttons[0].Kind = ButtonPredefines.Close;
            buttonEdit1.BorderStyle = BorderStyles.Simple;
            buttonEdit1.ForeColor = Color.White;
            buttonEdit1.Properties.Appearance.BorderColor = Color.White;
            buttonEdit1.Properties.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
            buttonEdit1.Properties.Appearance.TextOptions.WordWrap = WordWrap.Wrap;
            buttonEdit1.Properties.AutoHeight = true;
            buttonEdit1.ReadOnly = true;
            buttonEdit1.ButtonClick += removeTag_ButtonClick;
            buttonEdit1.Text = "TESTabcdefghijklmnTESTabcdefghijklmnTESTabcdefghijklmn";
            //// for testing ButtonEdit


        }

        private void removeTag_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            ButtonEdit btn = sender as ButtonEdit;
            panelTag.Controls.Remove(btn);

        }

        private void InitializeScenarioTypeList()
        {
            cboScenarioType.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;

            cboScenarioType.Properties.Items.Add("Import Module Scenario");
            cboScenarioType.Properties.Items.Add("Buffeting Scenario");
            cboScenarioType.Properties.Items.Add("ShortBlock Scenario");

            cboScenarioType.SelectedIndex = 0;
               

#if null
            BarManager barManager1 = new BarManager();
            barManager1.Form = this;

            PopupMenu popupMenu1 = new PopupMenu(barManager1);
            BarButtonItem btnImportModule = new BarButtonItem(barManager1, "Import Module Scenario");
            BarButtonItem btnBuffeting = new BarButtonItem(barManager1, "Buffeting Scenario");
            BarButtonItem btnShortBlock = new BarButtonItem(barManager1, "ShortBlock Scenario");
            popupMenu1.AddItem(btnImportModule);
            popupMenu1.AddItem(btnBuffeting);
            popupMenu1.AddItem(btnShortBlock);

            // 
            // dropScenarioType
            // 
            dropScenarioType.Parent = this;
            dropScenarioType.DropDownControl = popupMenu1;
            dropScenarioType.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            dropScenarioType.ImageOptions.ImageToTextIndent = 10;
            dropScenarioType.Click += new System.EventHandler(this.dropScenarioType_Click);

            // 
            // btnImportModule
            // 
            //btnImportModule.ImageOptions.SvgImage = global::WindowsFormsApplication1.Properties.Resources.zoomin;
            btnImportModule.Tag = "Import Module Scenario";
            btnImportModule.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnImportModule_ItemClick);

            // 
            // btnBuffeting
            // 
            //btnBuffeting.ImageOptions.SvgImage = global::WindowsFormsApplication1.Properties.Resources.zoomout;
            btnBuffeting.Tag = "Buffeting Scenario";
            btnBuffeting.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnBuffeting_ItemClick);

            // 
            // btnShortBlock
            // 
            //btnShortBlock.ImageOptions.SvgImage = global::WindowsFormsApplication1.Properties.Resources.zoomout;
            btnShortBlock.Tag = "ShortBlock Scenario";
            btnShortBlock.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnShortBlock_ItemClick);

#endif
        }

        private void InitializeFuselageList()
        {
#if null
            // radio 버튼을 좌에서 우로 순서대로 배열
            rgFuselage.Properties.ItemsLayout = RadioGroupItemsLayout.Flow;
            rgFuselage.AutoSizeInLayoutControl = false;
            //radioGroup1.Properties.EnableFocusRect = false;

            object[] itemValues = new object[] { 10, 11, 12, 13, 14 };
            string[] itemDescriptions = new string[] { "형상 A/1호기", "형상 A/2호기", "형상 A/3호기", "형상 B/1호기", "형상 C/1호기" };
            for (int i = 0; i < itemValues.Length; i++)
            {
                rgFuselage.Properties.Items.Add(new RadioGroupItem(itemValues[i], itemDescriptions[i]));
            }

            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 C/2호기"));
            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 C/2호기"));
            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 C/2호기"));
            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 C/2호기"));
            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 C/2호기"));
            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 C/2호기"));
            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 C/2호기"));
            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 C/2호기"));
            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 C/2호기"));
            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 C/2호기"));
            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 C/2호기"));
            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 C/2호기"));
            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 C/2호기"));
            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 C/2호기"));
            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 C/2호기"));
            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 C/2호기"));
            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 C/2호기"));
            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 C/2호기"));
            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 C/2호기"));
            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 C/2호기"));
            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 C/2호기"));
            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 C/2호기"));
            rgFuselage.Properties.Items.Add(new RadioGroupItem(1, "형상 Z/2호기"));

            //Select the Rectangle item.
            rgFuselage.EditValue = -1;
#endif
            AddFuselage("형상 A/1호기");
            AddFuselage("형상 A/2호기");
            AddFuselage("형상 A/3호기");
            AddFuselage("형상 B/1호기");
            AddFuselage("형상 C/1호기");
            AddFuselage("형상 D/1호기");
            AddFuselage("형상 E/1호기");
            AddFuselage("형상 F/1호기");
            AddFuselage("형상 G/1호기");
            AddFuselage("형상 H/1호기");
            AddFuselage("형상 Z/2호기");

        }

        private void AddFuselage(string name)
        {
            TextEdit edit = new TextEdit();
            edit.BorderStyle = BorderStyles.Simple;
            edit.ForeColor = Color.Gray;
            edit.Properties.Appearance.BorderColor = Color.Gray;
            edit.Properties.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
            edit.ReadOnly = true;
            //edit.Properties.AllowFocused = false;
            edit.Text = name;
            edit.Click += Fuselage_Click;
            panelFuseLage.Controls.Add(edit);
        }

        private void Fuselage_Click(object sender, EventArgs e)
        {
            TextEdit edit = sender as TextEdit;
            if (edit != null)
            {
                if(selectedFuselage.Equals(edit.Text))
                {
                    return;
                }

                // 하나의 비행기체만 선택되도록 한다.
                foreach(Control c in panelFuseLage.Controls)
                {
                    TextEdit ed = c as TextEdit;
                    ed.ForeColor = Color.Gray;
                    ed.Properties.Appearance.BorderColor = Color.Gray;
                    ed.Font = new Font(ed.Font, FontStyle.Regular);
                }
                edit.ForeColor = Color.White;
                edit.Properties.Appearance.BorderColor = Color.White;
                edit.Font = new Font(edit.Font, FontStyle.Bold);
                selectedFuselage = edit.Text;
            }
        }

        private void dropScenarioType_Click(object sender, EventArgs e)
        {
            string tag = (sender as DropDownButton).Tag.ToString();
            if (tag == "Import Module Scenario")
            {
            }
            if (tag == "Buffeting Scenario")
            {
            }
            if (tag == "ShortBlock Scenario")
            {
            }
        }


        private void btnImportModule_ItemClick(object sender, ItemClickEventArgs e)
        {
            UpdateDropDownButton(e.Item);
        }

        private void btnBuffeting_ItemClick(object sender, ItemClickEventArgs e)
        {
            UpdateDropDownButton(e.Item);
        }

        private void btnShortBlock_ItemClick(object sender, ItemClickEventArgs e)
        {
            UpdateDropDownButton(e.Item);
        }


        private void UpdateDropDownButton(BarItem submenuItem)
        {
            dropScenarioType.Text = submenuItem.Caption;
            dropScenarioType.ImageOptions.SvgImage = submenuItem.ImageOptions.SvgImage;
            dropScenarioType.ImageOptions.SvgImageSize = new Size(16, 16);
            dropScenarioType.Tag = submenuItem.Tag;
        }


        private void RemovePlaceholder(object sender, EventArgs e)
        {
            TextEdit txt = (TextEdit)sender;
            if (txt.Text == strInputScenarioName | txt.Text == strAddTag)
            { //텍스트박스 내용이 사용자가 입력한 값이 아닌 Placeholder일 경우에만, 커서 포커스일때 빈칸으로 만들기
                txt.ForeColor = Color.White; //사용자 입력 진한 글씨
                txt.Text = string.Empty;
            }
        }

        private void SetPlaceholder(object sender, EventArgs e)
        {
            TextEdit txt = (TextEdit)sender;
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                //사용자 입력값이 하나도 없는 경우에 포커스 잃으면 Placeholder 적용해주기
                txt.ForeColor = Color.DarkGray; //Placeholder 흐린 글씨
                if (txt == edtScenarioName) txt.Text = strInputScenarioName;
                else if (txt == edtTag) { txt.Text = strAddTag; }
            }
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit edit = sender as CheckEdit;
            if(edit != null)
            {
                if(edit.Checked)
                {
                    //edit.BackColor = Color.White;
                    edit.ForeColor = Color.White;
                    edit.Properties.Appearance.BorderColor = Color.White;
                }
                else
                {
                    //edit.BackColor = Color.Black;
                    edit.ForeColor = Color.Gray;
                    edit.Properties.Appearance.BorderColor = Color.Gray;
                }
            }
        }

        private void edtScenarioName_ClearButtonClick(object sender, ButtonPressedEventArgs e)
        {
            edtScenarioName.Text = String.Empty;
        }

        private void edtTag_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            ButtonEdit me = sender as ButtonEdit;
            if (me != null)
            {
                addTag(me.Text);
                me.Text = String.Empty;
            }

        }

        private void edtTag_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            ButtonEdit me = sender as ButtonEdit;
            if (me != null)
            {
                addTag(me.Text);
                me.Text = String.Empty;
            }
        }

        private void addTag(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            ButtonEdit btn = new ButtonEdit();
            btn.Properties.Buttons[0].Kind = ButtonPredefines.Close;
            btn.BorderStyle = BorderStyles.Simple;
            btn.ForeColor = Color.White;
            btn.Properties.Appearance.BorderColor = Color.White;
            btn.Font = new Font(btn.Font, FontStyle.Bold);
            btn.Properties.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
            btn.ReadOnly = true;
            btn.ButtonClick += removeTag_ButtonClick;
            btn.Text = name;
            panelTag.Controls.Add(btn);
        }

        private void cboScenarioType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit combo = sender as ComboBoxEdit;

            if (combo != null)
            {
                Console.WriteLine(combo.SelectedIndex);
                Console.WriteLine(combo.SelectedText);
            }
        }
    }

    public class CustomFormPainter : FormPainter
    {
        public CustomFormPainter(Control owner, DevExpress.Skins.ISkinProvider provider)
            : base(owner, provider)
        {
        }
        public CustomFormPainter(XtraForm owner, DevExpress.LookAndFeel.UserLookAndFeel provider, HorzAlignment captionAlignment)
            : base(owner, provider)
        {
            CaptionAlignment = captionAlignment;
        }
        private HorzAlignment _CaptionAlignment = HorzAlignment.Default;
        public HorzAlignment CaptionAlignment
        {
            get
            {
                return _CaptionAlignment;
            }
            set
            {
                _CaptionAlignment = value;
            }
        }
        protected override void DrawText(DevExpress.Utils.Drawing.GraphicsCache cache)
        {
            string text = Text;
            if (text == null || text.Length == 0 || TextBounds.IsEmpty)
            {
                return;
            }
            AppearanceObject appearance = new AppearanceObject(GetDefaultAppearance());
            appearance.TextOptions.Trimming = Trimming.EllipsisCharacter;
            appearance.TextOptions.HAlignment = CaptionAlignment;
            if (AllowHtmlDraw)
            {
                DrawHtmlText(cache, appearance);
                return;
            }
            Rectangle r = RectangleHelper.GetCenterBounds(TextBounds, new Size(TextBounds.Width, CalcTextHeight(cache, appearance)));
            DrawTextShadow(cache, appearance, r);
            cache.DrawString(text, appearance.Font, appearance.GetForeBrush(cache), r, appearance.GetStringFormat());
        }
    }

}