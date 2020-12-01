using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp4
{

    public class Danger
    {
        private int _id;
        private string _name;
        private string _description;
        private string _sourceOfDanger;
        private string _objectOfImpact;
        private bool _privacyViolation;
        private bool _integrityViolation;
        private bool _availableViolation;
        private DateTime _lastUpdateTime;

        public string ID
        {
            get { return "УБИ." + _id.ToString(); }
        }
        public string Name
        {
            get { return _name; }
        }
        public Danger(int id, string name, string description, string sourceOfDanger, string objectOfImpact, bool privacyViolation, bool integrityViolation, bool availableViolation, DateTime lastUpdateTime)
        {
            _id = id;
            _name = name;
            _description = description;
            _sourceOfDanger = sourceOfDanger;
            _objectOfImpact = objectOfImpact;
            _privacyViolation = privacyViolation;
            _integrityViolation = integrityViolation;
            _availableViolation = availableViolation;
            _lastUpdateTime = lastUpdateTime;
        }
        public override string ToString()
        {
            return $"Идентификатор УБИ - { _id}\n\n" +
                $"Наименование УБИ - { _name}\n\n" +
                $"Описание - {_description}\n\n" +
                $"Источник угрозы (характеристика и потенциал нарушителя) - {_sourceOfDanger}\n\n" +
                $"Объект воздействия - {_objectOfImpact}\n\n" +
                $"Нарушение конфиденциальности - {(_privacyViolation ? "Да" : "Нет")}\n\n" +
                $"Нарушение целостности - {(_integrityViolation ? "Да" : "Нет")}\n\n" +
                $"Нарушение доступности - {(_availableViolation ? "Да" : "Нет")}\n\n";
        }

        public override bool Equals(object obj)
        {
            Danger danger = obj as Danger;

            if (danger != null && danger._id == _id && danger._name == _name && danger._description == _description &&
                danger._sourceOfDanger == _sourceOfDanger && danger._objectOfImpact == _objectOfImpact &&
                danger._privacyViolation == _privacyViolation && danger._integrityViolation == _integrityViolation &&
                danger._availableViolation == _availableViolation && danger._lastUpdateTime == _lastUpdateTime)
            {
                return true;
            }
            else 
            { 
                return false; 
            }
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public string GetChangedFields(Danger danger)
        {
            string res = "Изменение в угрозе с ID = " + _id + '\n';
            if (danger._name != _name)
                res += $"Наименование УБИ:\nБЫЛО:\n{_name}\nСТАЛО:\n{danger._name}\n";
            if (danger._description != _description)
                res += $"Описание:\nБЫЛО:\n{_description}\nСТАЛО:\n{danger._description}\n";
            if (danger._sourceOfDanger != _sourceOfDanger)
                res += $"Источник угрозы:\nБЫЛО:\n{_sourceOfDanger}\nСТАЛО:\n{danger._sourceOfDanger}\n";
            if (danger._objectOfImpact != _objectOfImpact)
                res += $"Объект воздействия:\nБЫЛО:\n{_objectOfImpact}\nСТАЛО:\n{danger._objectOfImpact}\n";
            if (danger._privacyViolation != _privacyViolation)
                res += $"Нарушение конфиденциальности:\nБЫЛО:\n{_privacyViolation}\nСТАЛО:\n{danger._privacyViolation}\n";
            if (danger._integrityViolation != _integrityViolation)
                res += $"Нарушение целостности:\nБЫЛО:\n{_integrityViolation}\nСТАЛО:\n{danger._integrityViolation}\n";
            if (danger._availableViolation != _availableViolation)
                res += $"Нарушение доступности:\nБЫЛО:\n{_availableViolation}\nСТАЛО:\n{danger._availableViolation}\n";
            return res;
        }


    }

}
