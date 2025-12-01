# Simulador de Sistema Operacional — WPF (.NET)

**⚠️ IMPORTANTE:**  
Para que os processos carreguem e atualizem corretamente:  
- **Carregue o arquivo TXT duas vezes** pelo menu **Arquivo → Carregar Configuração**.  
- Para visualizar novos processos criados, vá em **Simulação → Executar Ciclo**.  
  Somente após executar um ciclo o novo processo aparecerá corretamente no **Escalonador**, **Memória** e no **Relatório Final**.  
- O projeto **não permite a criação de threads**, apenas processos.

---

## 📌 Sobre o Projeto

Este projeto implementa um **Simulador de Sistema Operacional** com interface gráfica desenvolvida em **WPF**, permitindo visualizar de forma interativa o funcionamento dos principais módulos de um SO:

- Gerenciamento de processos  
- Visualização de threads  
- Escalonamento  
- Memória  
- Dispositivos de Entrada/Saída  
- Sistema de Arquivos  
- Métricas detalhadas e relatório final  

A solução é dividida em dois componentes principais:

- **SimuladorSOInterface** – Interface gráfica WPF  
- **SimuladorSOLogica** – Núcleo do sistema operacional

---

# 📁 Estrutura da Solução
**SimuladorSOInterface**
Interface visual desenvolvida em **WPF**.

**SimuladorSOLogica**  
Contém toda a lógica do simulador — o “núcleo do SO”.
Cada diretório representa um módulo real de um sistema operacional:
- **Processos** – ciclo de vida dos processos  
- **Threads** – estruturas internas  
- **Escalonamento** – FCFS, Round Robin, Prioridades  
- **Memória** – páginas, molduras, TLB, políticas de alocação  
- **Entrada/Saída** – dispositivos simulados  
- **Sistema de Arquivos** – diretórios e blocos  
- **Métricas** – cálculo e geração de relatório final  

---

# ▶️ Como Usar

1️ Inicie o programa 
2️ Vá em **Arquivo → Carregar Configuração**  
⚠ **Carregue o arquivo TXT duas vezes**.

3️ Execute um ciclo da simulação  
Menu: **Simulação → Executar Ciclo**

4️ Observe as abas:
- Processos  
- Escalonador  
- Memória  
- Entrada/Saída  
- Sistema de Arquivos  

## 5️⃣ Gere o relatório final  
Menu: **Métricas → Gerar Relatório**

---
## 📷 Imagens da Aplicação

A pasta simulador contém **3 imagens** e **3 GIFs** demonstrando a execução e as telas do simulador. 

![Tela principal do simulador](Simulador/img1.png)  
*Figura 1 — Carregando o arquivo texto.*

![Analise do simulador](Simulador/img2.png)  
*Figura 2 — Como o simulador analisa o arquivo texto.*

![Relatório do simulador](Simulador/img3.png)  
*Figura 3 — Relatório gerado pelo arquivo texto.*

### Animações (GIFs)
![Execução passo-a-passo](Simulador/gif1.gif)  
*GIF 1 — Carregando o arquivo texto.*

![Gerenciando um processo](Simulador/gif2.gif)  
*GIF 2 — Adicionando e gerenciando um novo processo.*

![Gerenciando uma thread](Simulador/gif3.gif)  
*GIF 3 — Criando e gerenciando uma nova thread.*

---
## 🎯 Objetivo do Projeto

Este simulador foi desenvolvido com os seguintes objetivos:

- Demonstrar de forma didática e visual como funciona o interior de um Sistema Operacional.
- Implementar uma arquitetura modular semelhante a sistemas reais.
- Permitir experimentação e estudos de algoritmos de escalonamento, memória e E/S.
- Facilitar o aprendizado através de menus e simulação interativa.

---

## ▶️ Como Executar

1. Clone o repositório:
```bash
git clone https://github.com/CarolineGrizante/SimuladorSO




