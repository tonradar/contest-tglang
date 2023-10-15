using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;
using TgLang.CodeCrawler.Models;
using TgLang.CodeCrawler.Services.Contracts;

namespace TgLang.CodeCrawler.Services.Implementations
{
    public class LanguageDefService : ILanguageDefService
    {
        /// <summary>
        /// Gets all defined languages
        /// </summary>
        /// <returns></returns>
        public List<LanguageDef> GetLanguageDefs()
        {
            return LanguageDefs;
        }

        /// <summary>
        /// Extracts the file extension and according language from a GitHub file url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public (LanguageDef? language, string? extension) GetLanguageOfUrl(string url)
        {
            var parts = url.Split('/');
            if (parts.Any() && parts.Last().Contains("."))
            {
                var fileParts = parts.Last().Split('.');
                var extension = fileParts.Last();

                return (LanguageDefs.FirstOrDefault(l => l.Extension == extension), extension);
            }

            return (null, null);
        }

        private List<LanguageDef> LanguageDefs { get; } = new()
        {
            new(001, "1S_ENTERPRISE",   null,                   null                                                ),
            new(002, "ABAP",            "abap",                 Language.Abap                                       ),
            new(003, "ACTIONSCRIPT",    "as",                   Language.ActionScript                               ),
            new(004, "ADA",             "ada",                  Language.Ada                                        ),
            new(005, "APACHE_GROOVY",   "groovy",               Language.Groovy                                     ),
            new(006, "APEX",            "cls",                  Language.Apex                                       ),
            new(007, "APPLESCRIPT",     "applescript",          Language.AppleScript                                ),
            new(008, "ASP",             "aspx",                 null                                                ),
            new(009, "ASSEMBLY",        "asm",                  Language.Assembly                                   ),
            new(010, "AUTOHOTKEY",      "ahk",                  Language.AutoHotKey                                 ),
            new(011, "AWK",             "awk",                  Language.Awk                                        ),
            new(012, "BASIC",           "bas",                  null                                                ),
            new(013, "BATCH",           "bat",                  Language.Batchfile                                  ),
            new(014, "BISON",           null,                   null                                                ),
            new(015, "C",               "c",                    Language.C                                          ),
            new(016, "CLOJURE",         "clj",                  Language.Clojure                                    ),
            new(017, "CMAKE",           "cmake",                Language.Cmake                                      ),
            new(018, "COBOL",           "cob",                  Language.Cobol                                      ),
            new(019, "COFFESCRIPT",     "coffee",               Language.CoffeeScript                               ),
            new(020, "COMMON_LISP",     "gcl",                  null                                                ),
            new(021, "CPLUSPLUS",       "cpp",                  Language.CPlusPlus                                  ),
            new(022, "CRYSTAL",         "cr",                   null                                                ),
            new(023, "CSHARP",          "cs",                   Language.CSharp                                     ),
            new(024, "CSS",             "css",                  Language.Css                                        ),
            new(025, "CSV",             "csv",                  null                                                ),
            new(026, "D",               "d",                    Language.D                                          ),
            new(027, "DART",            "dart",                 Language.Dart                                       ),
            new(028, "DELPHI",          null,                   null                                                ),
            new(029, "DOCKER",          "dockerfile",           null                                                ),
            new(030, "ELIXIR",          "ex",                   Language.Elixir                                     ),
            new(031, "ELM",             "elm",                  Language.Elm                                        ),
            new(032, "ERLANG",          "erl",                  Language.Erlang                                     ),
            new(033, "FIFT",            "fif",                  null                                                ),
            new(034, "FORTH",           "forth",                Language.Forth,              isActive: false        ),
            new(035, "FORTRAN",         "for",                  Language.Fortran                                    ),
            new(036, "FSHARP",          "fs",                   null                                                ),
            new(037, "FUNC",            "func",                 null                                                ),
            new(038, "GAMS",            "gms",                  null                                                ),
            new(039, "GO",              "go",                   Language.Go                                         ),
            new(040, "GRADLE",          "gradle",               null                                                ),
            new(041, "GRAPHQL",         "graphql",              null                                                ),
            new(042, "HACK",            "hack",                 null                                                ),
            new(043, "HASKELL",         "hs",                   Language.Haskell                                    ),
            new(044, "HTML",            "html",                 null                                                ),
            new(045, "ICON",            "icn",                  null,                        isActive: false        ),
            new(046, "IDL",             "pro",                  null                                                ),
            new(047, "INI",             "ini",                  Language.Ini                                        ),
            new(048, "JAVA",            "java",                 Language.Java                                       ),
            new(049, "JAVASCRIPT",      "js",                   Language.JavaScript                                 ),
            new(050, "JSON",            "json",                 Language.Json                                       ),
            new(051, "JULIA",           "jl",                   Language.Julia                                      ),
            new(052, "KEYMAN",          "kmn",                  null                                                ),
            new(053, "KOTLIN",          "kt",                   Language.Kotlin                                     ),
            new(054, "LATEX",           "tex",                  Language.TeX                                        ),
            new(055, "LISP",            "lisp",                 null                                                ),
            new(056, "LOGO",            "lgo",                  null,                        isActive: false        ),
            new(057, "LUA",             "lua",                  Language.Lua                                        ),
            new(058, "MAKEFILE",        "mk",                   Language.Makefile                                   ),
            new(059, "MARKDOWN",        "md",                   Language.Markdown                                   ),
            new(060, "MATLAB",          "m",                    Language.Matlab                                     ),
            new(061, "NGINX",           "nginx",                Language.Nginx                                      ),
            new(062, "NIM",             "nim",                  null                                                ),
            new(063, "OBJECTIVE_C",     "m",                    Language.ObjectiveC                                 ),
            new(064, "OCAML",           "ml",                   Language.OCaml                                      ),
            new(065, "OPENEDGE_ABL",    "p",                    Language.OpenEdgeAbl                                ),
            new(066, "PASCAL",          "pas",                  Language.Pascal                                     ),
            new(067, "PERL",            "pl",                   Language.Perl                                       ),
            new(068, "PHP",             "php",                  Language.Php                                        ),
            new(069, "PL_SQL",          "plsql",                null                                                ),
            new(070, "POWERSHELL",      "ps1",                  Language.PowerShell                                 ),
            new(071, "PROLOG",          "pl",                   Language.Prolog                                     ),
            new(072, "PROTOBUF",        "proto",                null                                                ),
            new(073, "PYTHON",          "py",                   Language.Python                                     ),
            new(074, "QML",             "qml",                  null                                                ),
            new(075, "R",               "r",                    Language.R                                          ),
            new(076, "RAKU",            "raku",                 null                                                ),
            new(077, "REGEX",           "regex",                null                                                ),
            new(078, "RUBY",            "rb",                   Language.Ruby                                       ),
            new(079, "RUST",            "rs",                   Language.Rust                                       ),
            new(080, "SAS",             "sas",                  null                                                ),
            new(081, "SCALA",           "scala",                Language.Scala                                      ),
            new(082, "SCHEME",          "scm",                  Language.Scheme                                     ),
            new(083, "SHELL",           "sh",                   Language.Shell                                      ),
            new(084, "SMALLTALK",       "st",                   Language.Smalltalk                                  ),
            new(085, "SOLIDITY",        "sol",                  null                                                ),
            new(086, "SQL",             "sql",                  null                                                ),
            new(087, "SWIFT",           "swift",                Language.Swift                                      ),
            new(088, "TCL",             "tcl",                  Language.Tcl                                        ),
            new(089, "TEXTILE",         "textile",              Language.Textile                                    ),
            new(090, "TL",              "tl",                   null                                                ),
            new(091, "TYPESCRIPT",      "ts",                   Language.TypeScript                                 ),
            new(092, "UNREALSCRIPT",    "uc",                   null                                                ),
            new(093, "VALA",            "vala",                 Language.Vala                                       ),
            new(094, "VBSCRIPT",        "vbscript",             null                                                ),
            new(095, "VERILOG",         "v",                    Language.Verilog                                    ),
            new(096, "VISUAL_BASIC",    "vb",                   null                                                ),
            new(097, "WOLFRAM",         "wls",                  null                                                ),
            new(098, "XML",             "xml",                  Language.Xml                                        ),
            new(099, "YAML",            "yml",                  Language.Yaml                                       ),
        };
    }
}
