using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace rosneft.ModelView
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
    internal class VMMain : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string prop="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


        private double _discont = 0.2;
        public object Discont
        {
            get { return _discont; }
            set
            {
                double tmp;
                if(double.TryParse((string)value, NumberStyles.Any,
          CultureInfo.InvariantCulture, out tmp) && tmp >= 0){
                    Eror = Visibility.Hidden;
                    _discont = tmp;
                }
                else
                {
                    _discont = 0;
                    Eror = Visibility.Visible;
                    Res = 0;
                }
                OnPropertyChanged("Discont");
            }
        }


        private Visibility _eror = Visibility.Hidden;
        public Visibility Eror
        {
            get { return _eror; }
            set
            {
                _eror = value;
                OnPropertyChanged("Eror");
            }
        }


        private double _res = 0;
        public double Res
        {
            get { return _res; }
            set
            {
                if (Eror == Visibility.Hidden)
                {
                    _res = value;
                }
                else
                {
                    _res = 0;
                }
                OnPropertyChanged("Res");
            }
        }


        private List<int> _years;
        public List<int> Years
        {
            get { return _years; }
        }


        private int _last_year = 2050;
        public int Last_year
        {
            get { return _last_year; }
            set
            {
                _last_year = value;
                OnPropertyChanged("Last_year");
            }
        }


        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      Res = r.calculate((double)Discont, _last_year);
                      Res = Math.Round(_res, 2);
                      OnPropertyChanged("Res");
                  }));
            }
        }


        private Model.Model r;

        public VMMain()
        {
            r = new Model.Model();
            _years = r.Years;
        }
    }
}
