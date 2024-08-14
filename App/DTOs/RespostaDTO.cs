namespace Kcal.DTOs;

public abstract class RespostaDTO
{
    public bool Sucesso { get; set; }

    protected RespostaDTO() { }
}

// Se sucesso:
public class SucessoDto<T> : RespostaDTO
{
    public T? Data { get; set; }

    public SucessoDto(T? data)
    {
        Sucesso = true;
        Data = data;
    }
}

// Se erro:
public class ErroDto : RespostaDTO
{
    public string? MensagemErro { get; set; }

    public ErroDto(string me)
    {
        Sucesso = false;
        MensagemErro = me;
    }
}