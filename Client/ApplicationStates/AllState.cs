namespace Client.ApplicationStates
{
    public class AllState
    {
        // Scoped action is used to pass the action to the component
        public Action? Action { get; set; }
        public bool ShowGeneralDepartment { get; set; }
        public void GeneralDepartmentClicked()
        {
            ResetAllDepartment();
            ShowGeneralDepartment = true;
            Action?.Invoke();
        }

        // Department
        public bool ShowDepartment { get; set; }
        public void DepartmentClicked()
        {
            ResetAllDepartment();
            ShowDepartment = true;
            Action?.Invoke();
        }

        // Branch
        public bool ShowBranch { get; set; }
        public void BranchClicked()
        {
            ResetAllDepartment();
            ShowBranch = true;
            Action?.Invoke();
        }

        // Country
        public bool ShowCountry { get; set; }
        public void CountryClicked()
        {
            ResetAllDepartment();
            ShowCountry = true;
            Action?.Invoke();
        }

        // City
        public bool ShowCity { get; set; }
        public void CityClicked()
        {
            ResetAllDepartment();
            ShowCity = true;
            Action?.Invoke();
        }

        // Town
        public bool ShowTown { get; set; }
        public void TownClicked()
        {
            ResetAllDepartment();
            ShowTown = true;
            Action?.Invoke();
        }

        // User
        public bool ShowUser { get; set; }
        public void UserClicked()
        {
            ResetAllDepartment();
            ShowUser = true;
            Action?.Invoke();
        }

        // Employee
        public bool ShowEmployee { get; set; }
        public void EmployeeClicked()
        {
            ResetAllDepartment();
            ShowEmployee = true;
            Action?.Invoke();
        }

        // Doctor
        public bool ShowHealth { get; set; } = false;
        public void HealthClicked()
        {
            ResetAllDepartment();
            ShowHealth = true;
            Action?.Invoke();
        }

        // Overtime type
        public bool ShowOvertimeType { get; set; } = false;
        public void OvertimeTypeClicked()
        {
            ResetAllDepartment();
            ShowOvertimeType = true;
            Action?.Invoke();
        }

        // Overtime
        public bool ShowOvertime { get; set; } = false;
        public void OvertimeClicked()
        {
            ResetAllDepartment();
            ShowOvertime = true;
            Action?.Invoke();
        }

        // Sanction type
        public bool ShowSanctionType { get; set; } = false;
        public void SanctionTypeClicked()
        {
            ResetAllDepartment();
            ShowSanctionType = true;
            Action?.Invoke();
        }

        // Sanction
        public bool ShowSanction { get; set; } = false;
        public void SanctionClicked()
        {
            ResetAllDepartment();
            ShowSanction = true;
            Action?.Invoke();
        }

        // Vacation type
        public bool ShowVacationType { get; set; } = false;
        public void VacationTypeClicked()
        {
            ResetAllDepartment();
            ShowVacationType = true;
            Action?.Invoke();
        }

        // Vacation
        public bool ShowVacation { get; set; } = false;
        public void VacationClicked()
        {
            ResetAllDepartment();
            ShowVacation = true;
            Action?.Invoke();
        }

        private void ResetAllDepartment()
        {
            ShowGeneralDepartment = false;
            ShowDepartment = false;
            ShowBranch = false;
            ShowCountry = false;
            ShowCity = false;
            ShowTown = false;
            ShowUser = false;
            ShowEmployee = false;
            ShowHealth = false;
            ShowOvertimeType = false;
            ShowOvertime = false;
            ShowSanctionType = false;
            ShowSanction = false;
            ShowVacationType = false;
            ShowVacation = false;
        }
    }
}
