using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Curso.Domain
{
    public class Departamento
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public Departamento() { }

        // Primeira Opção

        private Action<object, string> _lazyLoader { get; set; }
        private Departamento(Action<object, string> lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        private List<Funcionario> _funcionarios;

        public List<Funcionario> Funcionarios
        {
            get
            {
                _lazyLoader?.Invoke(this, nameof(Funcionarios));

                return _funcionarios;
            }
            set => _funcionarios = value;
        }

        // Segunda opção

        // private ILazyLoader _lazyLoader { get; set; }
        // private Departamento(ILazyLoader lazyLoader)
        // {
        //     _lazyLoader = lazyLoader;
        // }

        // private List<Funcionario> _funcionarios;

        // public List<Funcionario> Funcionarios
        // {
        //     get => _lazyLoader.Load(this, ref _funcionarios);
        //     set => _funcionarios = value;
        // }
    }
}