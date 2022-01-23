﻿namespace FedjazContest.Models
{
    public class NavbarItem
    {
        public string Text { get; set; } = "";
        public string Controller { get; set; } = "";
        public string Action { get; set; } = "Index";
        public bool IsActive { get; set; } = false;

        public NavbarItem(string text, string controller, string action)
        {
            Text = text;
            Controller = controller;
            Action = action;
        }

        public NavbarItem(string text, string controller)
        {
            Text = text;
            Controller = controller;
        }
    }
}
