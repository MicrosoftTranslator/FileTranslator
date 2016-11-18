﻿// // ----------------------------------------------------------------------
// // <copyright file="SetCredentials.cs" company="Microsoft Corporation">
// // Copyright (c) Microsoft Corporation.
// // All rights reserved.
// // THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// // KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// // IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// // PARTICULAR PURPOSE.
// // </copyright>
// // ----------------------------------------------------------------------
// // <summary>SetCredentials.cs</summary>
// // ----------------------------------------------------------------------

namespace TranslationAssistant.AutomationToolkit.TranslationPlugins
{
    using System;
    using System.IO;
    using System.Linq;

    using TranslationAssistant.AutomationToolkit.BasePlugin;
    using TranslationAssistant.Business;
    using TranslationAssistant.Business.Model;
    using TranslationAssistant.TranslationServices.Core;

    /// <summary>
    ///     To set the credentials for the translation service.
    /// </summary>
    internal class SetCredentials : BasePlugIn
    {
        #region Fields

        /// <summary>
        ///     The source document.
        /// </summary>
        private readonly Argument clientID;

        /// <summary>
        ///     The source language.
        /// </summary>
        private readonly Argument clientSecret;
        
        /// <summary>
        ///     The subscription key.
        /// </summary>
        private readonly Argument subscriptionKey;

        /// <summary>
        ///     The target language.
        /// </summary>
        private readonly Argument categoryID;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SetCredentials" /> class.
        /// </summary>
        /// <param name="Logger">
        ///     The logger.
        /// </param>
        public SetCredentials(ConsoleLogger Logger)
            : base(Logger)
        {
            this.clientID = new Argument(
                "clientID",
                false,
                "Client ID to use for the calls to the Translator service. This is required if you don't have subscription key from Azure portal.");

            this.clientSecret = new Argument( 
                "clientSecret",
                false,
                "Client secret to use for the calls to the Translator service. This is required if you don't have subscription key from Azure portal.");

            this.categoryID = new Argument(
                "categoryID",
                false,
                "Translator Hub category ID to use for calls to the translator service.");

            this.subscriptionKey = new Argument(
                "subscriptionKey",
                false,
                "Subscription key to use for the calls to the Translator service. This is required if you don't have client ID and client secret from datamarket.");

            this.Arguments = new ArgumentList(
                new[] { this.clientID, this.clientSecret, this.categoryID, subscriptionKey },
                Logger);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the description.
        /// </summary>
        public override string Description
        {
            get
            {
                return "Sets the credentials for use with the Translator service.";
            }
        }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public override string Name
        {
            get
            {
                return "SetCredentials";
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The execute.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public override bool Execute()
        {
            try
            {
                TranslationServiceFacade.ClientID = this.clientID.ValueString;
                TranslationServiceFacade.ClientSecret = this.clientSecret.ValueString;
                TranslationServiceFacade.CategoryID = this.categoryID.ValueString;
                TranslationServiceFacade.SubscriptionKey = this.subscriptionKey.ValueString;
                TranslationServiceFacade.SaveCredentials();
            }
            catch (Exception ex)
            {
                this.Logger.WriteException(ex);
                Console.ReadLine();
                return false;
            }

            this.Logger.WriteLine(LogLevel.Msg, string.Format("Credentials saved."));
            if (TranslationServiceFacade.IsTranslationServiceReady())
            {
                this.Logger.WriteLine(LogLevel.Msg, string.Format("Translator service is ready to use."));
            }
            else
            {
                this.Logger.WriteLine(LogLevel.Error, string.Format("Credentials are invalid."));
            }
            return true;
        }

        #endregion
    }
}