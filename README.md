# Descrição das versões dos softwares utilizados

- Sistema Operacional: Windows 10 Pro
- Ferramenta de Desenvolvimento: Visual Studio 2017 (15.3.5)
- Biblioteca: Microsoft .Net Framework 4.7.1
- Linguagem: C# 6.0
- PostgreSQL: 9.3.19

# Arquitetura da solução 

A aplicação será monolítica com exceção dos dados. Estes serão armazenados e gerenciados por um Sistema de Gerenciamento de Banco de Dados (SGBD).

A comunicação entre a aplicação e o SGBD se dará por meio da canal seguro SSL.

A aplicação será uma aplicativo console, desenvolvido para plataforma Windows.


# Plano de testes
	
- O sistema deve permitir que o administrador da votação seja capaz de cadastrar candidatos previamente (antes de se iniciarem as votações).
- O sistema deve permitir que seja emitido o relatório mostrando que todos os candidatos estão com 0 (zero) votos.
- Não deve ser possível emitir relatórios parciais e nem cadastrar novos candidatos após o início das votações.
- Não é permitido que uma pessoa vote mais de uma vez.
- Após encerrar a votação, o sistema deve permitir que o auditor da votação (e apenas o auditor) emita o relatório de votos com a contagem dos votos recebidos por cada candidato.
- O sistema deve produzir logs de auditoria para cada ação realizada no sistema (autenticação, votação, etc) sem que o candidato votado seja identificável pelo registro.