﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConEmuIntegration.Properties {
    using System;
    
    
    /// <summary>
    ///   Eine stark typisierte Ressourcenklasse zum Suchen von lokalisierten Zeichenfolgen usw.
    /// </summary>
    // Diese Klasse wurde von der StronglyTypedResourceBuilder automatisch generiert
    // -Klasse über ein Tool wie ResGen oder Visual Studio automatisch generiert.
    // Um einen Member hinzuzufügen oder zu entfernen, bearbeiten Sie die .ResX-Datei und führen dann ResGen
    // mit der /str-Option erneut aus, oder Sie erstellen Ihr VS-Projekt neu.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Gibt die zwischengespeicherte ResourceManager-Instanz zurück, die von dieser Klasse verwendet wird.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ConEmuIntegration.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Überschreibt die CurrentUICulture-Eigenschaft des aktuellen Threads für alle
        ///   Ressourcenzuordnungen, die diese stark typisierte Ressourcenklasse verwenden.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///&lt;key name=&quot;Software&quot;&gt;
        ///    &lt;key name=&quot;ConEmu&quot;&gt;
        ///        &lt;key name=&quot;.Vanilla&quot; modified=&quot;2016-07-04 23:35:45&quot; build=&quot;160619&quot;&gt;
        ///            &lt;value name=&quot;StartType&quot; type=&quot;hex&quot; data=&quot;02&quot;/&gt;
        ///            &lt;value name=&quot;CmdLine&quot; type=&quot;string&quot; data=&quot;&quot;/&gt;
        ///            &lt;value name=&quot;StartTasksFile&quot; type=&quot;string&quot; data=&quot;&quot;/&gt;
        ///            &lt;value name=&quot;StartTasksName&quot; type=&quot;string&quot; data=&quot;{Shells::PowerShell}&quot;/&gt;
        ///            &lt;value name=&quot;StartFarFolders&quot; type=&quot;hex&quot; data=&quot;00&quot;/&gt;
        ///           [Rest der Zeichenfolge wurde abgeschnitten]&quot;; ähnelt.
        /// </summary>
        internal static string ConEmu {
            get {
                return ResourceManager.GetString("ConEmu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die Path ähnelt.
        /// </summary>
        internal static string SettingsConEmuPath {
            get {
                return ResourceManager.GetString("SettingsConEmuPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die Default task ähnelt.
        /// </summary>
        internal static string SettingsDefaultTask {
            get {
                return ResourceManager.GetString("SettingsDefaultTask", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die XML configuration ähnelt.
        /// </summary>
        internal static string SettingsXMLConfiguration {
            get {
                return ResourceManager.GetString("SettingsXMLConfiguration", resourceCulture);
            }
        }
    }
}
