using System;
using System.Linq;

public struct Paciente
{
    public string NomeCompleto;
    public string Mae;
    public string DataNascimento;
    public char Sexo;
    public string CPF;
}

public struct Atendimento
{
    public Paciente Paciente;
    public string Data;
    public string Procedimento;
    public int DuracaoMinutos;
}

public struct ClinicaCDI
{
    public Paciente[] Pacientes;
    public Atendimento[] Atendimentos;
    public int NumPacientes;
    public int NumAtendimentos;
}

class Program
{
    static void Main()
    {
        ClinicaCDI clinica = new ClinicaCDI();
        InicializarClinica(ref clinica);

        int opcao;

        do
        {
            ExibirMenu();
            Console.Write("Opção: ");
            opcao = int.Parse(Console.ReadLine());

            switch (opcao)
            {
                case 1:
                    CadPaciente(ref clinica);
                    break;
                case 2:
                    CadAtendimento(ref clinica);
                    break;
                case 3:
                    ListarPacientes(clinica);
                    break;
                case 4:
                    Console.Write("Data (DD/MM/AAAA): ");
                    string data = Console.ReadLine();
                    ListarAtendPorData(clinica, data);
                    break;
                case 5:
                    Console.Write("Procedimento: ");
                    string proc5 = Console.ReadLine();
                    Console.Write("Data inicial (DD/MM/AAAA): ");
                    string ini5 = Console.ReadLine();
                    Console.Write("Data final (DD/MM/AAAA): ");
                    string fin5 = Console.ReadLine();
                    ObterNumProced(clinica, proc5, ini5, fin5);
                    break;
                case 6:
                    Console.Write("Procedimento: ");
                    string proc6 = Console.ReadLine();
                    Console.Write("Data inicial (DD/MM/AAAA): ");
                    string ini6 = Console.ReadLine();
                    Console.Write("Data final (DD/MM/AAAA): ");
                    string fin6 = Console.ReadLine();
                    ObterTempDurProc(clinica, proc6, ini6, fin6);
                    break;
                case 7:
                    Console.Write("Data inicial (DD/MM/AAAA): ");
                    string ini7 = Console.ReadLine();
                    Console.Write("Data final (DD/MM/AAAA): ");
                    string fin7 = Console.ReadLine();
                    ObterTempDurTodosProcs(clinica, ini7, fin7);
                    break;
                case 8:
                    EscreverMsg("Saindo do programa...");
                    break;
                default:
                    EscreverMsg("Opção inválida. Tente novamente.");
                    break;
            }

        } while (opcao != 8);

        Console.ReadLine();
    }

    static void InicializarClinica(ref ClinicaCDI clinica)
    {
        clinica.Pacientes = null;
        clinica.Atendimentos = null;
        clinica.NumPacientes = 0;
        clinica.NumAtendimentos = 0;
    }

    static void CadPaciente(ref ClinicaCDI clinica)
    {
        clinica.NumPacientes++;
        Array.Resize(ref clinica.Pacientes, clinica.NumPacientes);

        Paciente novoPaciente = new Paciente();

        Console.Write("Nome: ");
        novoPaciente.NomeCompleto = Console.ReadLine();

        Console.Write("Nome da Mãe: ");
        novoPaciente.Mae = Console.ReadLine();

        Console.Write("Data de Nascimento (DD/MM/AAAA): ");
        novoPaciente.DataNascimento = Console.ReadLine();

        if (CalcIdade(novoPaciente.DataNascimento) < 12)
        {
            EscreverMsg("Idade inferior a 12 anos. Cadastro não permitido.", true);
            return;
        }

        Console.Write("Sexo (M/F): ");
        novoPaciente.Sexo = char.Parse(Console.ReadLine());

        Console.Write("CPF: ");
        novoPaciente.CPF = Console.ReadLine();

        clinica.Pacientes[clinica.NumPacientes - 1] = novoPaciente;

        EscreverMsg($"Paciente {novoPaciente.NomeCompleto} cadastrado com sucesso!");
    }

    static void CadAtendimento(ref ClinicaCDI clinica)
    {
        clinica.NumAtendimentos++;
        Array.Resize(ref clinica.Atendimentos, clinica.NumAtendimentos);

        Atendimento novoAtendimento = new Atendimento();

        Console.WriteLine("Paciente:");
        for (int i = 0; i < clinica.NumPacientes; i++)
        {
            Console.WriteLine($"{i + 1}. {clinica.Pacientes[i].NomeCompleto}");
        }

        int escolhaPaciente;
        Console.Write("Escolha o número correspondente ao paciente: ");
        escolhaPaciente = int.Parse(Console.ReadLine());

        if (escolhaPaciente <= 0 || escolhaPaciente > clinica.NumPacientes)
        {
            EscreverMsg("Escolha inválida.", true);
            return;
        }

        novoAtendimento.Paciente = clinica.Pacientes[escolhaPaciente - 1];

        Console.Write("Data do Atendimento (DD/MM/AAAA): ");
        novoAtendimento.Data = Console.ReadLine();

        Console.WriteLine("Procedimento:");
        Console.WriteLine("1. Raios-X de Tórax em PA");
        Console.WriteLine("2. Ultrassonografia Obstétrica");
        Console.WriteLine("3. Ultrassonografia de Próstata");
        Console.WriteLine("4. Tomografia");

        int escolhaProcedimento;
        Console.Write("Escolha o número correspondente ao procedimento: ");
        escolhaProcedimento = int.Parse(Console.ReadLine());

        switch (escolhaProcedimento)
        {
            case 1:
                novoAtendimento.Procedimento = "Raios-X de Tórax em PA";
                break;
            case 2:
                if (novoAtendimento.Paciente.Sexo == 'M')
                {
                    EscreverMsg("Esse procedimento é apenas para mulheres", true);
                    return;
                }
                else
                {
                    novoAtendimento.Procedimento = "Ultrassonografia Obstétrica";
                }
                break;
            case 3:
                if (novoAtendimento.Paciente.Sexo == 'F')
                {
                    EscreverMsg("Esse procedimento é apenas para homens", true);
                    return;
                }
                else
                {
                    novoAtendimento.Procedimento = "Ultrassonografia de Próstata";
                }
                break;
            case 4:
                if (VerifUltrassonRecente(clinica, novoAtendimento.Paciente, "Ultrassonografia Obstétrica") ||
                    VerifUltrassonRecente(clinica, novoAtendimento.Paciente, "Ultrassonografia de Próstata"))
                {
                    EscreverMsg("Tomografia não pode ser realizada, pois o paciente realizou Ultrassonografia Obstétrica ou Ultrassonografia de Próstata nos últimos três meses.", true);
                    return;
                }
                novoAtendimento.Procedimento = "Tomografia";
                break;
            default:
                EscreverMsg("Escolha inválida.", true);
                return;
        }

        Console.Write("Duração em minutos: ");
        novoAtendimento.DuracaoMinutos = int.Parse(Console.ReadLine());

        clinica.Atendimentos[clinica.NumAtendimentos - 1] = novoAtendimento;

        EscreverMsg($"Atendimento registrado com sucesso para o paciente {novoAtendimento.Paciente.NomeCompleto} na data {novoAtendimento.Data}.");
    }

    static void ListarPacientes(ClinicaCDI clinica)
    {
        for (int i = 0; i < clinica.NumPacientes; i++)
        {
            EscreverMsg($"Nome: {clinica.Pacientes[i].NomeCompleto}, Data de Nascimento: {clinica.Pacientes[i].DataNascimento}");
        }
    }

    static void ListarAtendPorData(ClinicaCDI clinica, string data)
    {
        for (int i = 0; i < clinica.NumAtendimentos; i++)
        {
            if (string.Equals(clinica.Atendimentos[i].Data, data))
            {
                EscreverMsg($"Paciente: {clinica.Atendimentos[i].Paciente.NomeCompleto}, Data de Nascimento: {clinica.Atendimentos[i].Paciente.DataNascimento}, Procedimento: {clinica.Atendimentos[i].Procedimento}");
            }
        }
    }

    static int ObterNumProced(ClinicaCDI clinica, string procedimento, string dataInicial, string dataFinal)
    {
        int qtdProcedimentos = 0;
        for (int i = 0; i < clinica.NumAtendimentos; i++)
        {
            if (string.Equals(clinica.Atendimentos[i].Procedimento, procedimento) &&
                string.Compare(clinica.Atendimentos[i].Data, dataInicial) >= 0 &&
                string.Compare(clinica.Atendimentos[i].Data, dataFinal) <= 0)
            {
                qtdProcedimentos++;
            }
        }

        EscreverMsg($"Número de procedimentos '{procedimento}' realizados no período: {qtdProcedimentos}");
        return qtdProcedimentos;
    }

    static void ObterTempDurProc(ClinicaCDI clinica, string procedimento, string dataInicial, string dataFinal)
    {
        int totalDuracao = 0;
        for (int i = 0; i < clinica.NumAtendimentos; i++)
        {
            if (string.Equals(clinica.Atendimentos[i].Procedimento, procedimento) &&
                string.Compare(clinica.Atendimentos[i].Data, dataInicial) >= 0 &&
                string.Compare(clinica.Atendimentos[i].Data, dataFinal) <= 0)
            {
                totalDuracao += clinica.Atendimentos[i].DuracaoMinutos;
            }
        }

        EscreverMsg($"Tempo total de duração do procedimento '{procedimento}' no período: {totalDuracao} minutos");
    }

    static void ObterTempDurTodosProcs(ClinicaCDI clinica, string dataInicial, string dataFinal)
    {
        int totalDuracao = 0;
        for (int i = 0; i < clinica.NumAtendimentos; i++)
        {
            if (string.Compare(clinica.Atendimentos[i].Data, dataInicial) >= 0 &&
                string.Compare(clinica.Atendimentos[i].Data, dataFinal) <= 0)
            {
                totalDuracao += clinica.Atendimentos[i].DuracaoMinutos;
            }
        }

        EscreverMsg($"Tempo total de duração de todos os procedimentos no período: {totalDuracao} minutos");
    }

    static void ExibirMenu()
    {
        Console.WriteLine("=======================================");
        Console.WriteLine("|            MENU PRINCIPAL            |");
        Console.WriteLine("=======================================");
        Console.WriteLine("| 1. Cadastrar Paciente               |");
        Console.WriteLine("| 2. Cadastrar Atendimento            |");
        Console.WriteLine("| 3. Listar Pacientes                 |");
        Console.WriteLine("| 4. Listar Atendimentos por Data     |");
        Console.WriteLine("| 5. Número de Procedimentos em um    |");
        Console.WriteLine("|    Período                           |");
        Console.WriteLine("| 6. Tempo Total de Duração de um     |");
        Console.WriteLine("|    Procedimento em um Período       |");
        Console.WriteLine("| 7. Tempo Total de Duração de Todos  |");
        Console.WriteLine("|    Procedimentos em um Período      |");
        Console.WriteLine("| 8. Sair                             |");
        Console.WriteLine("=======================================");
        Console.Write("Opção: ");
    }

    static void EscreverMsg(string mensagem, bool destaque = false)
    {
        if (destaque)
        {
            Console.WriteLine($"!!! {mensagem} !!!");
        }
        else
        {
            Console.WriteLine($"- {mensagem}");
        }
    }

    static int CalcIdade(string dataNascimento)
    {
        DateTime dataNasc = DateTime.ParseExact(dataNascimento, "dd/MM/yyyy", null);
        DateTime dataAtual = DateTime.Now;
        int idade = dataAtual.Year - dataNasc.Year;

        if (dataAtual.Month < dataNasc.Month || (dataAtual.Month == dataNasc.Month && dataAtual.Day < dataNasc.Day))
        {
            idade--;
        }

        return idade;
    }

    static bool VerifUltrassonRecente(ClinicaCDI clinica, Paciente paciente, string procedimento)
    {
        DateTime dataAtual = DateTime.Now;

        for (int i = 0; i < clinica.NumAtendimentos; i++)
        {
            if (string.Equals(clinica.Atendimentos[i].Paciente.NomeCompleto, paciente.NomeCompleto, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(clinica.Atendimentos[i].Procedimento, procedimento, StringComparison.OrdinalIgnoreCase) &&
                (dataAtual - DateTime.ParseExact(clinica.Atendimentos[i].Data, "dd/MM/yyyy", null)).TotalDays <= 90)
            {
                return true;
            }
        }

        return false;
    }
}
