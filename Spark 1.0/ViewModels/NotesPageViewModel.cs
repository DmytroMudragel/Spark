using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Collections.ObjectModel;
using System.Linq;
using MvvmHelpers;
using System.Windows.Input;
using Spark.Models;
using MvvmHelpers.Commands;
using System.Threading.Tasks;
using Xamarin.Forms;
using Spark.Views;
using Spark.Services;

namespace Spark.ViewModels
{
    public class NotesPageViewModel : BaseViewModel
    {
        public ICommand AddNewNoteCommand { get; set; }

        public ICommand SelectedNoteCommand { get; set; }

        public ObservableRangeCollection<Note> Notes { get; set; }

        public MvvmHelpers.Commands.Command<Note> SelectedCommand { get; set; }

        public AsyncCommand SaveNotesCommand { get; }

        public AsyncCommand DeleteNotesCommand { get; }

        public ICommand BackFromNoteEditorCommand { get; }

        public ICommand Set1ColorCommand { get; }

        public ICommand Set2ColorCommand { get; }

        public ICommand Set3ColorCommand { get; }

        public ICommand Set4ColorCommand { get; }

        public ICommand Set5ColorCommand { get; }

        public ICommand Set6ColorCommand { get; }

        public ICommand Set7ColorCommand { get; }


        public bool isExistedNoteEditing = false;

        public Note currSelected { get; set; }

        public NotesPageViewModel()
        {
            AddNewNoteCommand = new MvvmHelpers.Commands.Command(AddNewNoteBtnClick);
            BackFromNoteEditorCommand = new MvvmHelpers.Commands.Command(BackFromNoteEditorBtnClick);
            SaveNotesCommand = new AsyncCommand(SaveNoteBtnClick);
            DeleteNotesCommand = new AsyncCommand(DeleteNoteBtnClick);
            Set1ColorCommand = new MvvmHelpers.Commands.Command(Set1ColorClick);
            Set2ColorCommand = new MvvmHelpers.Commands.Command(Set2ColorClick);
            Set3ColorCommand = new MvvmHelpers.Commands.Command(Set3ColorClick);
            Set4ColorCommand = new MvvmHelpers.Commands.Command(Set4ColorClick);
            Set5ColorCommand = new MvvmHelpers.Commands.Command(Set5ColorClick);
            Set6ColorCommand = new MvvmHelpers.Commands.Command(Set6ColorClick);
            Set7ColorCommand = new MvvmHelpers.Commands.Command(Set7ColorClick);
            Notes = new ObservableRangeCollection<Note>();
            Task.Run(async () => await RefreshNotes());
        }

        #region NoteEditor

        public static string GetHexString(Color color)
        {
            var red = (int)(color.R * 255);
            var green = (int)(color.G * 255);
            var blue = (int)(color.B * 255);
            var alpha = (int)(color.A * 255);
            var hex = $"#{alpha:X2}{red:X2}{green:X2}{blue:X2}";
            return hex;
        }

        private async Task DeleteNoteBtnClick()
        {
            await NoteService.RemoveNote(currSelected.Id);
            await RefreshNotes();
            BackFromNoteEditorBtnClick();
        }

        private async Task SaveNoteBtnClick()
        {
            if (noteEditorEditorTitle != "" || noteEditorEditorText != "")
            {
                var notescount = await NoteService.GetAllNotes();
                if (notescount.Count() != 0)
                {
                    var currNote = await NoteService.GetNote(currSelected.Id);
                    if (currNote != null)
                    {
                        currSelected.Id = currNote.Id;
                    }
                }
                Note newNote = new Note()
                {
                    Id = currSelected.Id,
                    NoteTitle = noteEditorEditorTitle,
                    NoteText = noteEditorEditorText,
                    NoteColor = GetHexString(noteChosenColor),
                    NoteTextColor = GetHexString((Color)Application.Current.Resources["PrimaryDark"])
                };
                await NoteService.AddNote(newNote);
                BackFromNoteEditorBtnClick();
                await RefreshNotes();
            }
            else
            {
                BackFromNoteEditorBtnClick();
            }  
        }

        private async Task RefreshNotes()
        {
            //IsBusy = true;
            //await Task.Delay(500);
            if (Notes.Count.Equals(null))
            {
                return;
            }

            Notes.Clear();
            Notes.AddRange(await NoteService.GetAllNotes());
            //IsBusy = false;
        }

        private void BackFromNoteEditorBtnClick()
        {
            IsNewNoteEditorPageIsActive = false;
            NoteEditorEditorTitle = "";
            NoteEditorEditorText = "";
            NoteChosenColor = (Color)Application.Current.Resources["NoteColorBeige"];
        }

        private void AddNewNoteBtnClick()
        {
            currSelected = new Note() { };
            IsNewNoteEditorPageIsActive = true;
        }

        private void NoteIsSelected(Note currentSelectedNote)
        {
            currSelected = currentSelectedNote;
            isExistedNoteEditing = true;
            IsNewNoteEditorPageIsActive = true;
            NoteEditorEditorTitle = currSelected.NoteTitle;
            NoteEditorEditorText = currSelected.NoteText;
            currSelected.NoteTextColor = GetHexString((Color)Application.Current.Resources["PrimaryDark"]);
            NoteChosenColor = Color.FromHex(currSelected.NoteColor);
        }

        private Note selctednote;
        public Note SelectedNote
        {
            get => selctednote;
            set
            {
                if (value != null)
                {
                    NoteIsSelected(value);
                    value = null;
                }
                selctednote = value;
                OnPropertyChanged(nameof(SelectedNote));
            }
        }

        private string noteEditorEditorTitle = "";
        public string NoteEditorEditorTitle
        {
            get => noteEditorEditorTitle;
            set => SetProperty(ref noteEditorEditorTitle, value);
        }

        private string noteEditorEditorText = "";
        public string NoteEditorEditorText
        {
            get => noteEditorEditorText;
            set => SetProperty(ref noteEditorEditorText, value);
        }

        private bool isNewNoteEditorPageIsActive = false;
        public bool IsNewNoteEditorPageIsActive
        {
            get => isNewNoteEditorPageIsActive;
            set => SetProperty(ref isNewNoteEditorPageIsActive, value);
        }

        #region Colors Circles

        private Color noteChosenColor = (Color)Application.Current.Resources["NoteColorBeige"];
        public Color NoteChosenColor
        {
            get => noteChosenColor;
            set => SetProperty(ref noteChosenColor, value);
        }


        private Color color1 = (Color)Application.Current.Resources["NoteColorBeige"];
        public Color Color1
        {
            get => color1;
            set => SetProperty(ref color1, value);
        }
        private void Set1ColorClick()
        {
            NoteChosenColor = Color1;

        }

        private Color color2 = (Color)Application.Current.Resources["NoteColorOrange"];

        public Color Color2
        {
            get => color2;
            set => SetProperty(ref color2, value);
        }
        private void Set2ColorClick()
        {
            NoteChosenColor = Color2;
        }

        private Color color3 = (Color)Application.Current.Resources["NoteColorYellow"];
        public Color Color3
        {
            get => color3;
            set => SetProperty(ref color3, value);
        }
        private void Set3ColorClick()
        {
            NoteChosenColor = Color3;
        }

        private Color color4 = (Color)Application.Current.Resources["NoteColorLightGreen"];
        public Color Color4
        {
            get => color4;
            set => SetProperty(ref color4, value);
        }
        private void Set4ColorClick()
        {
            NoteChosenColor = Color4;
        }

        private Color color5 = (Color)Application.Current.Resources["NoteColorGreen"];
        public Color Color5
        {
            get => color5;
            set => SetProperty(ref color5, value);
        }
        private void Set5ColorClick()
        {
            NoteChosenColor = Color5;
        }

        private Color color6 = (Color)Application.Current.Resources["NoteColorLigthBlue"];
        public Color Color6
        {
            get => color6;
            set => SetProperty(ref color6, value);
        }
        private void Set6ColorClick()
        {
            NoteChosenColor = Color6;
        }

        private Color color7 = (Color)Application.Current.Resources["NoteColorPurple"];
        public Color Color7
        {
            get => color7;
            set => SetProperty(ref color7, value);
        }
        private void Set7ColorClick()
        {
            NoteChosenColor = Color7;
        }

        #endregion

        #endregion
    }
}
