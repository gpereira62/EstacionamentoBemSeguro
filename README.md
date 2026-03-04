<h1 align="center">🅿️ Estacionamento Bem Seguro</h1>

<p align="center">
  Aplicação console em C# para gerenciamento de estacionamento com suporte a motos, carros e vans.
</p>

<p align="center">
  <a href="#sobre">Sobre</a> •
  <a href="#funcionalidades">Funcionalidades</a> •
  <a href="#regras">Regras</a> •
  <a href="#tecnologias">Tecnologias</a> •
  <a href="#como-executar">Como Executar</a> •
  <a href="#testes">Testes</a> •
  <a href="#autor">Autor</a>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white" />
  <img src="https://img.shields.io/badge/xUnit-Testes-brightgreen?style=for-the-badge" />
  <img src="https://img.shields.io/badge/status-concluído-success?style=for-the-badge" />
</p>

---

## Sobre

O **Estacionamento Bem Seguro** é um sistema de gerenciamento de vagas para estacionamento via terminal. Ao iniciar, o operador configura a quantidade de vagas e o preço por hora. A partir daí, o sistema controla entradas, saídas, pagamentos e emite avisos automáticos sobre a ocupação.

---

## Funcionalidades

- ✅ Estacionar motos, carros e vans
- ✅ Encontrar a melhor vaga automaticamente por tipo de veículo
- ✅ Saída de veículo com cálculo de tempo e cobrança
- ✅ Listar todos os veículos estacionados
- ✅ Informar quantas vagas as vans estão ocupando
- ✅ Avisos automáticos (estacionamento cheio, vazio ou tipo esgotado)
- ✅ Controle de caixa em tempo real
- ✅ Tela explicativa com as regras do estacionamento

---

## Regras

### Tipos de Vagas

| Tipo    | Indicada para |
|---------|--------------|
| Pequena | Motos        |
| Média   | Carros       |
| Grande  | Vans         |

### Regras por Veículo

**Moto**
- Pode estacionar em qualquer vaga disponível (Pequena → Média → Grande)
- Ocupa apenas 1 vaga

**Carro**
- Pode estacionar em vagas Médias ou Grandes (Média → Grande)
- Não pode usar vagas Pequenas

**Van**
- Prioridade: 1 vaga Grande
- Alternativa: 3 vagas Médias (caso não haja vaga Grande disponível)
- Não pode entrar se não houver nem vaga Grande nem 3 Médias livres

### Cobrança

- Cobrado por hora cheia
- Mínimo de 1 hora (frações são arredondadas para baixo, mas nunca abaixo de 1h)
- Exemplo: 2h30min = cobra 2 horas

---

## Tecnologias

- [C#](https://docs.microsoft.com/pt-br/dotnet/csharp/)
- [.NET 9](https://learn.microsoft.com/pt-br/dotnet/core/whats-new/dotnet-9)
- [xUnit](https://xunit.net/) — testes unitários

---

## Como Executar

### Pré-requisitos

- [Git](https://git-scm.com)
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

### Passo a Passo

```bash
# Clone o repositório
git clone https://github.com/gpereira62/EstacionamentoBemSeguro.git

# Entre na pasta do projeto
cd EstacionamentoBemSeguro

# Execute a aplicação
dotnet run --project EstacionamentoBemSeguro
```

> Também é possível abrir o arquivo `EstacionamentoBemSeguro.sln` no Visual Studio 2022 ou VS Code e executar normalmente.

---

## Testes

O projeto possui testes unitários cobrindo as principais regras de negócio.

```bash
# Rodar todos os testes
dotnet test
```

Cenários cobertos:
- `PodeEstacionar` para cada tipo de veículo (com e sem vagas)
- `EstacionarVeiculo` — alocação correta de vagas por tipo
- `ExcluirVeiculo` — liberação de vagas (incluindo van em 3 médias)
- `InfoPagamentoSaida` — cálculo correto do valor cobrado
- `Aviso.ChecarAvisos` — alertas de lotação

---

## Autor

<a href="https://www.linkedin.com/in/gustavo-pereira-18302316a/">
  <img style="border-radius: 50%;" src="https://media-exp1.licdn.com/dms/image/C4D03AQFICCCMopiLcQ/profile-displayphoto-shrink_200_200/0/1569797034513?e=1634774400&v=beta&t=368E-ErqfgKrjdb6b0Duk07Ic1q9QFbL0vQRwnkq7Og" width="100px;" alt="Gustavo Pereira"/>
  <br />
  <sub><b>Gustavo Pereira</b></sub>
</a>

<br /><br />

Feito com ❤️ por Gustavo Pereira — Entre em contato!

<a href="https://www.linkedin.com/in/gustavo-pereira-18302316a/">
  <img src="https://img.shields.io/badge/linkedin-%230077B5.svg?&style=for-the-badge&logo=linkedin&logoColor=white" />
</a>&nbsp;
<a href="https://instagram.com/gustavops_dds">
  <img src="https://img.shields.io/badge/instagram-%23E4405F.svg?&style=for-the-badge&logo=instagram&logoColor=white" />
</a>&nbsp;
<a href="mailto:gustavopereirasantos@hotmail.com">
  <img src="https://img.shields.io/badge/Microsoft_Outlook-0078D4?style=for-the-badge&logo=microsoft-outlook&logoColor=white" />
</a>
