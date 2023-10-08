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
            new("1S_ENTERPRISE", null, null),
            new("ABAP", "abap", Language.Abap),
            new("ACTIONSCRIPT", "as", Language.ActionScript),
            new("ADA", "ada", Language.Ada),
            new("APACHE_GROOVY", "groovy", Language.Groovy),
            new("APEX", "cls", Language.Apex),
            new("APPLESCRIPT", "applescript", Language.AppleScript),
            new("ASP", "asp", Language.Asp),
            new("ASSEMBLY", "asm", Language.Assembly),
            new("AUTOHOTKEY", "ahk", Language.AutoHotKey),
            new("AWK", "awk", Language.Awk),
            new("BASIC", null, null),
            new("BATCH", "bat", Language.Batchfile),
            new("BISON", null, null),
            new("C", "c", Language.C),
            new("CLOJURE", "clj", Language.Clojure),
            new("CMAKE", null, Language.Cmake),
            new("COBOL", "cobol", Language.Cobol),
            new("COFFESCRIPT", "coffee", Language.CoffeeScript),
            new("COMMON_LISP", null, Language.CommonLisp),
            new("CPLUSPLUS", "cpp,", Language.CPlusPlus),
            new("CRYSTAL", null, null),
            new("CSHARP", "cs", Language.CSharp),
            new("CSS", "css", Language.Css),
            new("CSV", null, null),
            new("D", "d", Language.D),
            new("DART", "dart", Language.Dart),
            new("DELPHI", null, null),
            new("DOCKER", null, null),
            new("ELIXIR", "ex", Language.Elixir),
            new("ELM", "elm", Language.Elm),
            new("ERLANG", "erl", Language.Erlang),
            new("FIFT", "fif", null),
            new("FORTH", "forth", Language.Forth),
            new("FORTRAN", "for", Language.Fortran),
            new("FSHARP", "fs", Language.FSharp),
            new("FUNC", "func", null),
            new("GAMS", null, null),
            new("GO", "go", Language.Go),
            new("GRADLE", null, null),
            new("GRAPHQL", null, null),
            new("HACK", null, null),
            new("HASKELL", "hs", Language.Haskell),
            new("HTML", "html", null),
            new("ICON", null, null),
            new("IDL", "pro", null),
            new("INI", "ini", Language.Ini),
            new("JAVA", "java", Language.Java),
            new("JAVASCRIPT", "js", Language.JavaScript),
            new("JSON", "json", Language.Json),
            new("JULIA", "jl", Language.Julia),
            new("KEYMAN", null, null),
            new("KOTLIN", "kt", Language.Kotlin),
            new("LATEX", "tex", Language.TeX),
            new("LISP", "lisp", Language.CommonLisp),
            new("LOGO", null, null),
            new("LUA", "lua", Language.Lua),
            new("MAKEFILE", "mk", Language.Makefile),
            new("MARKDOWN", "md", Language.Markdown),
            new("MATLAB", "m", Language.Matlab),
            new("NGINX", null, Language.Nginx),
            new("NIM", "nim", Language.Nimrod),
            new("OBJECTIVE_C", "m", Language.ObjectiveC),
            new("OCAML", "ml", Language.OCaml),
            new("OPENEDGE_ABL", null, Language.OpenEdgeAbl),
            new("PASCAL", "pas", Language.Pascal),
            new("PERL", "pl", Language.Perl),
            new("PHP", "php", Language.Php),
            new("PL_SQL", null, null),
            new("POWERSHELL", null, Language.PowerShell),
            new("PROLOG", null, Language.Prolog),
            new("PROTOBUF", null, null),
            new("PYTHON", "py", Language.Python),
            new("QML", null, null),
            new("R", "r", Language.R),
            new("RAKU", null, null),
            new("REGEX", "regex", null),
            new("RUBY", "rb", Language.Ruby),
            new("RUST", "rs", Language.Rust),
            new("SAS", "sas", Language.Sass),
            new("SCALA", "scala", Language.Scala),
            new("SCHEME", "scm", Language.Scheme),
            new("SHELL", "sh", Language.Shell),
            new("SMALLTALK", "st", Language.Smalltalk),
            new("SOLIDITY", "sol", null),
            new("SQL", "sql", null),
            new("SWIFT", "swift", Language.Swift),
            new("TCL", "tcl", Language.Tcl),
            new("TEXTILE", "textile", Language.Textile),
            new("TL", null, null),
            new("TYPESCRIPT", "ts", Language.TypeScript),
            new("UNREALSCRIPT", null, null),
            new("VALA", "vala", Language.Vala),
            new("VBSCRIPT", null, null),
            new("VERILOG", "v", Language.Verilog),
            new("VISUAL_BASIC", "vb", Language.VisualBasic),
            new("WOLFRAM", null, null),
            new("XML", "xml", Language.Xml),
            new("YAML", "yml", Language.Yaml)
        };
    }
}
