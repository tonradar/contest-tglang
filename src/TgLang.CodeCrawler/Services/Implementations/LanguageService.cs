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
        public List<LanguageDef> GetLanguageDefs()
        {
            return LanguageDefs;
        }

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

        //private List<LanguageDef> LanguageDefs { get; set; } = new List<LanguageDef>()
        //{
        //    new("C#", "cs", "csharp", Language.CSharp),
        //    new("JavaScript", "js", "javascript", Language.JavaScript),
        //    new("TypeScript", "ts", "typescript", Language.TypeScript),
        //    new("XML", "xml", "xml", Language.Xml),
        //    new("CSS", "css", "css", Language.Css),
        //    new("Markdown", "md", "markdown", Language.Markdown),
        //    new("YAML", "yml", "yaml", Language.Yaml),
        //};


        private List<LanguageDef> LanguageDefs { get; set; } = new List<LanguageDef>()
        {
            new("1S_ENTERPRISE", "", null),
            new("ABAP", "", Language.Abap),
            new("ACTIONSCRIPT", "", Language.ActionScript),
            new("ADA", "", Language.Ada),
            new("APACHE_GROOVY", "", Language.Groovy),
            new("APEX", "", Language.Apex),
            new("APPLESCRIPT", "", Language.AppleScript),
            new("ASP", "", Language.Asp),
            new("ASSEMBLY", "", Language.Assembly),
            new("AUTOHOTKEY", "", Language.AutoHotKey),
            new("AWK", "", Language.Awk),
            new("BASIC", "", null),
            new("BATCH", "", Language.Batchfile),
            new("BISON", "", null),
            new("C", "", Language.C),
            new("CLOJURE", "", Language.Clojure),
            new("CMAKE", "", Language.Cmake),
            new("COBOL", "", Language.Cobol),
            new("COFFESCRIPT", "", Language.CoffeeScript),
            new("COMMON_LISP", "", Language.CommonLisp),
            new("CPLUSPLUS", "", Language.CPlusPlus),
            new("CRYSTAL", "", null),
            new("CSHARP", "", Language.CSharp),
            new("CSS", "", Language.Css),
            new("CSV", "", null),
            new("D", "", Language.D),
            new("DART", "", Language.Dart),
            new("DELPHI", "", null),
            new("DOCKER", "", null),
            new("ELIXIR", "", Language.Elixir),
            new("ELM", "", Language.Elm),
            new("ERLANG", "", Language.Erlang),
            new("FIFT", "", null),
            new("FORTH", "", Language.Forth),
            new("FORTRAN", "", Language.Fortran),
            new("FSHARP", "", Language.FSharp),
            new("FUNC", "", null),
            new("GAMS", "", null),
            new("GO", "", Language.Go),
            new("GRADLE", "", null),
            new("GRAPHQL", "", null),
            new("HACK", "", null),
            new("HASKELL", "", Language.Haskell),
            new("HTML", "", null),
            new("ICON", "", null),
            new("IDL", "", null),
            new("INI", "", Language.Ini),
            new("JAVA", "", Language.Java),
            new("JAVASCRIPT", "", Language.JavaScript),
            new("JSON", "", Language.Json),
            new("JULIA", "", Language.Julia),
            new("KEYMAN", "", null),
            new("KOTLIN", "", Language.Kotlin),
            new("LATEX", "", Language.TeX),
            new("LISP", "", null),
            new("LOGO", "", null),
            new("LUA", "", Language.Lua),
            new("MAKEFILE", "", Language.Makefile),
            new("MARKDOWN", "", Language.Markdown),
            new("MATLAB", "", Language.Matlab),
            new("NGINX", "", Language.Nginx),
            new("NIM", "", Language.Nimrod),
            new("OBJECTIVE_C", "", Language.ObjectiveC),
            new("OCAML", "", Language.OCaml),
            new("OPENEDGE_ABL", "", Language.OpenEdgeAbl),
            new("PASCAL", "", Language.Pascal),
            new("PERL", "", Language.Perl),
            new("PHP", "", Language.Php),
            new("PL_SQL", "", null),
            new("POWERSHELL", "", Language.PowerShell),
            new("PROLOG", "", Language.Prolog),
            new("PROTOBUF", "", null),
            new("PYTHON", "", Language.Python),
            new("QML", "", null),
            new("R", "", Language.R),
            new("RAKU", "", null),
            new("REGEX", "", null),
            new("RUBY", "", Language.Ruby),
            new("RUST", "", Language.Rust),
            new("SAS", "", Language.Sass),
            new("SCALA", "", Language.Scala),
            new("SCHEME", "", Language.Scheme),
            new("SHELL", "", Language.Shell),
            new("SMALLTALK", "", Language.Smalltalk),
            new("SOLIDITY", "", null),
            new("SQL", "", null),
            new("SWIFT", "", Language.Swift),
            new("TCL", "", Language.Tcl),
            new("TEXTILE", "", Language.Textile),
            new("TL", "", null),
            new("TYPESCRIPT", "", Language.TypeScript),
            new("UNREALSCRIPT", "", null),
            new("VALA", "", Language.Vala),
            new("VBSCRIPT", "", null),
            new("VERILOG", "", Language.Verilog),
            new("VISUAL_BASIC", "", Language.VisualBasic),
            new("WOLFRAM", "", null),
            new("XML", "", Language.Xml),
            new("YAML", "", Language.Yaml),
        };
    }
}
