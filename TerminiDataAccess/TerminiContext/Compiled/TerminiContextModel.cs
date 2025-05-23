﻿// <auto-generated />
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

#pragma warning disable 219, 612, 618
#nullable disable

namespace TerminiDataAccess.TerminiContext
{
    [DbContext(typeof(TerminiContext))]
    public partial class TerminiContextModel : RuntimeModel
    {
        private static readonly bool _useOldBehavior31751 =
            System.AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue31751", out var enabled31751) && enabled31751;

        static TerminiContextModel()
        {
            var model = new TerminiContextModel();

            if (_useOldBehavior31751)
            {
                model.Initialize();
            }
            else
            {
                var thread = new System.Threading.Thread(RunInitialization, 10 * 1024 * 1024);
                thread.Start();
                thread.Join();

                void RunInitialization()
                {
                    model.Initialize();
                }
            }

            model.Customize();
            _instance = (TerminiContextModel)model.FinalizeModel();
        }

        private static TerminiContextModel _instance;
        public static IModel Instance => _instance;

        partial void Initialize();

        partial void Customize();
    }
}
