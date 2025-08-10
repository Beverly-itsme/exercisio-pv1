using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Program3_Drone.cs
using System;

class Program3
{
    static void Main()
    {
        Drone drone = new Drone();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== CONTROLE DO DRONE ===");
            Console.WriteLine(drone.Status());
            Console.WriteLine();
            Console.WriteLine("1 - Definir altura (0.5 - 25)");
            Console.WriteLine("2 - Subir 0.5 m");
            Console.WriteLine("3 - Descer 0.5 m");
            Console.WriteLine("4 - Definir direção (0 - 359)");
            Console.WriteLine("5 - Girar esquerda 5°");
            Console.WriteLine("6 - Girar direita 5°");
            Console.WriteLine("7 - Aumentar velocidade (+0.5 m/s)");
            Console.WriteLine("8 - Diminuir velocidade (-0.5 m/s)");
            Console.WriteLine("9 - Aproximar de objeto");
            Console.WriteLine("10 - Distanciar do objeto");
            Console.WriteLine("11 - Menu braços");
            Console.WriteLine("0 - Sair");
            Console.Write("Escolha: ");
            string op = Console.ReadLine();

            if (op == "0") break;
            switch (op)
            {
                case "1":
                    Console.Write("Altura desejada: ");
                    if (double.TryParse(Console.ReadLine(), out double alt)) drone.DefinirAltura(alt);
                    else { Console.WriteLine("Valor inválido."); Pause(); }
                    break;
                case "2": drone.AlterarAltura(0.5); break;
                case "3": drone.AlterarAltura(-0.5); break;
                case "4":
                    Console.Write("Direção desejada (0-359): ");
                    if (int.TryParse(Console.ReadLine(), out int d)) drone.DefinirDirecao(d);
                    else { Console.WriteLine("Valor inválido."); Pause(); }
                    break;
                case "5": drone.AlterarDirecao(-5); break;
                case "6": drone.AlterarDirecao(5); break;
                case "7": drone.AlterarVelocidade(0.5); break;
                case "8": drone.AlterarVelocidade(-0.5); break;
                case "9": drone.AproximarObjeto(); break;
                case "10": drone.DistanciarObjeto(); break;
                case "11": MenuBracos(drone); break;
                default: Console.WriteLine("Opção inválida."); Pause(); break;
            }
        }
    }

    static void MenuBracos(Drone drone)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== MENU BRAÇOS ===");
            Console.WriteLine(drone.BracosStatus());
            Console.WriteLine();
            Console.WriteLine("1 - Selecionar braço esquerdo");
            Console.WriteLine("2 - Selecionar braço direito");
            Console.WriteLine("0 - Voltar");
            Console.Write("Escolha: ");
            string opt = Console.ReadLine();
            if (opt == "0") break;
            switch (opt)
            {
                case "1": SubMenuBraco(drone.Esquerdo, drone); break;
                case "2": SubMenuBraco(drone.Direito, drone); break;
                default: Console.WriteLine("Inválido."); Pause(); break;
            }
        }
    }

    static void SubMenuBraco(Braco braco, Drone drone)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== BRAÇO {(braco.IsLeft ? "ESQUERDO" : "DIREITO")} ===");
            Console.WriteLine(braco.Status());
            Console.WriteLine();
            Console.WriteLine("1 - Alternar cotovelo (Repouso <-> Contraído)");
            Console.WriteLine("2 - Alternar estado atividade do braço (Repouso <-> EmAtividade) -- só se permitido");
            Console.WriteLine("3 - Girar pulso +5°");
            Console.WriteLine("4 - Girar pulso -5°");
            Console.WriteLine("5 - Definir pulso para ângulo (0-359)");
            Console.WriteLine("6 - Pegar");
            Console.WriteLine("7 - Armazenar");
            Console.WriteLine("8 - Bater (apenas esquerdo)");
            Console.WriteLine("9 - Cortar (apenas direito)");
            Console.WriteLine("10 - Coletar (apenas direito)");
            Console.WriteLine("0 - Voltar");
            Console.Write("Escolha: ");
            string op = Console.ReadLine();
            if (op == "0") break;
            switch (op)
            {
                case "1": braco.ToggleCotovelo(); break;
                case "2": braco.ToggleAtividade(drone); break;
                case "3": braco.RodarPulso(5); break;
                case "4": braco.RodarPulso(-5); break;
                case "5":
                    Console.Write("Ângulo desejado (0-359): ");
                    if (int.TryParse(Console.ReadLine(), out int ang)) braco.DefinirPulso(ang);
                    else { Console.WriteLine("Valor inválido."); Pause(); }
                    break;
                case "6": braco.Pegar(); break;
                case "7": braco.Armazenar(); break;
                case "8":
                    if (!braco.IsLeft) { Console.WriteLine("Só o braço esquerdo tem Bater."); Pause(); }
                    else braco.Bater();
                    break;
                case "9":
                    if (braco.IsLeft) { Console.WriteLine("Só o braço direito tem Cortar."); Pause(); }
                    else braco.Cortar();
                    break;
                case "10":
                    if (braco.IsLeft) { Console.WriteLine("Só o braço direito tem Coletar."); Pause(); }
                    else braco.Coletar();
                    break;
                default: Console.WriteLine("Inválido."); Pause(); break;
            }
        }
    }

    static void Pause()
    {
        Console.WriteLine("Pressione Enter...");
        Console.ReadLine();
    }
}

/* ----------------- CLASSES DO DRONE E BRAÇOS ----------------- */

public class Drone
{
    // Altura entre 0.5 e 25
    public double Altura { get; private set; } = 0.5;
    // Direção 0..359 (0 = norte)
    public int Direcao { get; private set; } = 0;
    // Velocidade 0..15 (m/s)
    public double Velocidade { get; private set; } = 0.0;
    // Aproximado de objeto (quando true, bloqueia alterações de altura/direção/velocidade e impede nova aproximação)
    public bool AproximadoObjeto { get; private set; } = false;

    // Braços
    public Braco Esquerdo { get; private set; }
    public Braco Direito { get; private set; }

    public Drone()
    {
        Esquerdo = new Braco(true, this);
        Direito = new Braco(false, this);
    }

    public string Status()
    {
        string movimento = Velocidade > 0 ? "Em movimento" : "Sem movimento";
        return $"Altura: {Altura:F1} m | Direção: {Direcao}° | Velocidade: {Velocidade:F1} m/s ({movimento}) | Aproximado: {(AproximadoObjeto ? "Sim" : "Não")}";
    }

    public string BracosStatus()
    {
        return $"Braço Esquerdo: {Esquerdo.Resumo()} \nBraço Direito: {Direito.Resumo()}";
    }

    public void DefinirAltura(double nova)
    {
        if (AproximadoObjeto) { Msg("Não é possível mudar altura enquanto próximo de um objeto."); return; }
        if (nova < 0.5 || nova > 25) { Msg("Altura fora dos limites (0.5 - 25)."); return; }
        Altura = Math.Round(nova, 1);
        Msg($"Altura definida para {Altura:F1} m.");
    }

    public void AlterarAltura(double delta)
    {
        DefinirAltura(Altura + delta);
    }

    public void DefinirDirecao(int d)
    {
        if (AproximadoObjeto) { Msg("Não é possível mudar direção enquanto próximo de um objeto."); return; }
        Direcao = ((d % 360) + 360) % 360;
        Msg($"Direção definida para {Direcao}°.");
    }

    public void AlterarDirecao(int delta)
    {
        DefinirDirecao(Direcao + delta);
    }

    public void AlterarVelocidade(double delta)
    {
        if (AproximadoObjeto) { Msg("Não é possível mudar velocidade enquanto próximo de um objeto."); return; }
        double nova = Math.Round(Velocidade + delta, 1);
        if (nova < 0) nova = 0;
        if (nova > 15) { Msg("Velocidade máxima 15 m/s."); return; }
        Velocidade = nova;

        // Regra: se estiver em movimento, braços devem estar em Repouso
        if (Velocidade > 0)
        {
            Esquerdo.ForceRepousoPorMovimento();
            Direito.ForceRepousoPorMovimento();
        }

        Msg($"Velocidade agora {Velocidade:F1} m/s.");
    }

    public void AproximarObjeto()
    {
        if (AproximadoObjeto) { Msg("Já está próximo de um objeto."); return; }
        if (Velocidade > 0) { Msg("Pare o drone (velocidade 0) antes de aproximar-se do objeto."); return; }
        // Simular aproximação lenta...
        AproximadoObjeto = true;
        // Após aproximação o drone deve estar parado e não pode mudar altura/direção/velocidade
        Velocidade = 0;
        Msg("Drone aproximou-se do objeto. Funções de voo bloqueadas. Agora é possível usar braços.");
    }

    public void DistanciarObjeto()
    {
        if (!AproximadoObjeto) { Msg("Drone não está próximo de nenhum objeto."); return; }
        AproximadoObjeto = false;
        Msg("Drone se distanciou do objeto. Funções de voo liberadas.");
    }

    private void Msg(string s)
    {
        Console.WriteLine(s);
        Console.WriteLine("Pressione Enter...");
        Console.ReadLine();
    }
}

public enum CotoveloState { Repouso, Contraido }
public enum EstadoBraco { Repouso, EmAtividade, Ocupado }

public class Braco
{
    public bool IsLeft { get; private set; }
    public CotoveloState Cotovelo { get; private set; } = CotoveloState.Repouso;
    public EstadoBraco Estado { get; private set; } = EstadoBraco.Repouso;
    public int Pulso { get; private set; } = 0; // 0..359
    public Drone DroneRef { get; private set; }

    public Braco(bool isLeft, Drone drone)
    {
        IsLeft = isLeft;
        DroneRef = drone;
    }

    public string Resumo()
    {
        return $"{(IsLeft ? "Esq" : "Dir")} | Cotovelo: {Cotovelo} | Estado: {Estado} | Pulso: {Pulso}° | Ocupado: {(Estado == EstadoBraco.Ocupado ? "Sim" : "Não")}";
    }

    public string Status()
    {
        string permitido = DroneRef.AproximadoObjeto ? "Permite uso" : "Braços não podem ser usados (não aproximado)";
        if (DroneRef.Velocidade > 0) permitido = "Braços forçados a Repouso (drone em movimento)";
        return Resumo() + " | " + permitido;
    }

    // Toggle cotovelo com respeito às regras
    public void ToggleCotovelo()
    {
        // regra: o braço não pode ficar "Em repouso" se estiver no estado "Ocupado".
        if (Estado == EstadoBraco.Ocupado && Cotovelo == CotoveloState.Contraido)
        {
            Console.WriteLine("Não é possível colocar cotovelo em repouso enquanto braço ocupado. Armazene primeiro.");
            Pause(); return;
        }

        Cotovelo = Cotovelo == CotoveloState.Repouso ? CotoveloState.Contraido : CotoveloState.Repouso;
        Console.WriteLine($"Cotovelo agora: {Cotovelo}");
        Pause();
    }

    // Toggle atividade (somente se drone aproximado e velocidade 0 e não ocupada)
    public void ToggleAtividade(Drone drone)
    {
        if (!drone.AproximadoObjeto) { Console.WriteLine("Só é possível ativar braço após aproximação do objeto."); Pause(); return; }
        if (drone.Velocidade > 0) { Console.WriteLine("Não é possível ativar braço se drone estiver em movimento."); Pause(); return; }
        if (Estado == EstadoBraco.Ocupado) { Console.WriteLine("Braço está ocupado, não pode alternar para atividade agora."); Pause(); return; }

        Estado = Estado == EstadoBraco.Repouso ? EstadoBraco.EmAtividade : EstadoBraco.Repouso;
        Console.WriteLine($"Estado do braço agora: {Estado}");
        Pause();
    }

    public void RodarPulso(int delta)
    {
        if (!PermiteMovimento()) { Console.WriteLine("Não é permitido mover o pulso agora."); Pause(); return; }
        Pulso = ((Pulso + delta) % 360 + 360) % 360;
        Console.WriteLine($"Pulso: {Pulso}°");
        Pause();
    }

    public void DefinirPulso(int ang)
    {
        if (!PermiteMovimento()) { Console.WriteLine("Não é permitido mover o pulso agora."); Pause(); return; }
        if (ang < 0 || ang > 359) { Console.WriteLine("Ângulo inválido."); Pause(); return; }
        Pulso = ang;
        Console.WriteLine($"Pulso definido para {Pulso}°");
        Pause();
    }

    // Ferramentas:
    public void Pegar()
    {
        // Regra: só pode ser usada se cotovelo estiver contraído.
        if (!PermiteAcaoFerramenta()) { Console.WriteLine("Não é permitido usar ferramentas agora."); Pause(); return; }
        if (Cotovelo != CotoveloState.Contraido) { Console.WriteLine("Cotovelo deve estar contraído para pegar."); Pause(); return; }
        if (Estado == EstadoBraco.Ocupado) { Console.WriteLine("Braço já ocupa um objeto."); Pause(); return; }

        Estado = EstadoBraco.Ocupado;
        Console.WriteLine("Objeto pego. Braço agora Ocupado. Use 'Armazenar' para liberar.");
        Pause();
    }

    public void Armazenar()
    {
        // só pode ser usada se cotovelo estiver Em repouso e braço ocupado (segundo enunciado: "Armazenar: só pode ser utilizada se o cotovelo estiver Em repouso.")
        if (!PermiteAcaoFerramenta()) { Console.WriteLine("Não é permitido usar ferramentas agora."); Pause(); return; }
        if (Cotovelo != CotoveloState.Repouso) { Console.WriteLine("Cotovelo deve estar em Repouso para armazenar (levar ao recipiente)."); Pause(); return; }
        if (Estado != EstadoBraco.Ocupado) { Console.WriteLine("Braço não está ocupado; nada para armazenar."); Pause(); return; }

        Estado = EstadoBraco.Repouso; // libera o braço
        Console.WriteLine("Objeto armazenado. Braço liberado e em Repouso.");
        Pause();
    }

    public void Bater()
    {
        // só braço esquerdo: cotovelo contraído e braço desocupado
        if (!IsLeft) { Console.WriteLine("Bater disponível apenas no braço esquerdo."); Pause(); return; }
        if (!PermiteAcaoFerramenta()) { Console.WriteLine("Não é permitido usar ferramentas agora."); Pause(); return; }
        if (Cotovelo != CotoveloState.Contraido) { Console.WriteLine("Cotovelo deve estar contraído para bater."); Pause(); return; }
        if (Estado == EstadoBraco.Ocupado) { Console.WriteLine("Braço ocupado; não pode bater."); Pause(); return; }

        // simular bater
        Console.WriteLine("Executando: Bater (martelo) ...");
        Pause();
    }

    public void Cortar()
    {
        // só braço direito: cotovelo contraído e braço desocupado
        if (IsLeft) { Console.WriteLine("Cortar disponível apenas no braço direito."); Pause(); return; }
        if (!PermiteAcaoFerramenta()) { Console.WriteLine("Não é permitido usar ferramentas agora."); Pause(); return; }
        if (Cotovelo != CotoveloState.Contraido) { Console.WriteLine("Cotovelo deve estar contraído para cortar."); Pause(); return; }
        if (Estado == EstadoBraco.Ocupado) { Console.WriteLine("Braço ocupado; não pode cortar."); Pause(); return; }

        Console.WriteLine("Executando: Cortar (tesoura) ...");
        Pause();
    }

    public void Coletar()
    {
        // só braço direito: cotovelo Em Repouso e braço desocupado; ao coletar, braço fica Ocupado até armazenar
        if (IsLeft) { Console.WriteLine("Coletar disponível apenas no braço direito."); Pause(); return; }
        if (!PermiteAcaoFerramenta()) { Console.WriteLine("Não é permitido usar ferramentas agora."); Pause(); return; }
        if (Cotovelo != CotoveloState.Repouso) { Console.WriteLine("Cotovelo deve estar em Repouso para coletar."); Pause(); return; }
        if (Estado == EstadoBraco.Ocupado) { Console.WriteLine("Braço ocupado; não pode coletar."); Pause(); return; }

        Estado = EstadoBraco.Ocupado;
        Console.WriteLine("Coleta realizada. Braço agora Ocupado. Use 'Armazenar' depois.");
        Pause();
    }

    // Helpers e regras:
    private bool PermiteMovimento()
    {
        // só pode movimentar o pulso se drone está aproximado E parado e braço em EmAtividade
        if (!DroneRef.AproximadoObjeto) return false;
        if (DroneRef.Velocidade > 0) return false;
        if (Estado != EstadoBraco.EmAtividade) return false;
        return true;
    }

    private bool PermiteAcaoFerramenta()
    {
        // só após aproximação, drone parado, e braço em atividade
        if (!DroneRef.AproximadoObjeto) return false;
        if (DroneRef.Velocidade > 0) return false;
        if (Estado != EstadoBraco.EmAtividade && !(Estado == EstadoBraco.Ocupado && (/* allow Armazenar when Ocupado but cotovelo Repouso handled separately */ true)))
            return false;
        // operations like Pegar/Coletar require EmAtividade; Armazenar can be chamada quando Ocupado (handled externally)
        return Estado == EstadoBraco.EmAtividade || Estado == EstadoBraco.Ocupado;
    }

    // Forçar repouso quando drone em movimento
    public void ForceRepousoPorMovimento()
    {
        if (DroneRef.Velocidade > 0)
        {
            Estado = EstadoBraco.Repouso;
        }
    }

    private void Pause()
    {
        Console.WriteLine("Pressione Enter...");
        Console.ReadLine();
    }
}
