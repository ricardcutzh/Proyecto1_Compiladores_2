/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Abstract;

/**
 *
 * @author ricar
 */
import ManejoError.TError;
import Tablas.TablaSimbolos;
import Traductor.TablaOpciones;
import java.io.StringReader;
import java.util.ArrayList;
import java.util.HashMap;
import EtiquetaParser.*;
public class Pregunta implements ArbolForm{
    
    //ATRIBUTOS FIJOS
    String Idpregunta;
    String Etiqueta;
    int Tipo;
    
    //ATRIBUTOS OPCIONALES
    HashMap<String, Atributo> atributos;
    
    //PADRE DE LA PREGUNTA
    String padre = "";
    
    //
    ArrayList<String> tabs;
    TablaOpciones tablaOpciones;
    String idListaOpcion;
    String ficheroExt;
    
    //
    String visibilidadResp = "publico ";
    
    //METODO DE RESPUESTA
    String metodoResp = "";
    String metodoCalcular = "";
    String metodoMostrar = "";
    public Pregunta(String idpregunta, String etiqueta, int tipo)
    {
        this.Idpregunta = idpregunta;
        this.Etiqueta = etiqueta;
        this.Tipo = tipo;
        this.atributos = new HashMap<>();
        this.valorAtributosLocales = new ArrayList<>();
    }

    public void setPadre(String padre) {
        this.padre = padre;
    }

    public void setTablaOpciones(TablaOpciones tablaOpciones) {
        this.tablaOpciones = tablaOpciones;
    }

    public void setIdListaOpcion(String idListaOpcion) {
        this.idListaOpcion = idListaOpcion;
    }

    public void setFicheroExt(String ficheroExt) {
        this.ficheroExt = ficheroExt;
    }

    public String getIdpregunta() {
        return Idpregunta;
    }

    public String getEtiqueta() {
        return Etiqueta;
    }

    public int getTipo() {
        return Tipo;
    }
    
    public boolean addAtributo(Atributo att, String nombre)
    {
        if(!this.atributos.containsKey(nombre))
        {
            this.atributos.put(nombre, att);
            return true;
        }
        else
        {
            return false;
        }
    }
    
    ArrayList<String> parametrosDePreguntas;
    
    ArrayList<String> parametrosPadre;
    @Override
    public Object traducirLocal(TablaSimbolos ts, ArrayList<String> tabs, ArrayList<TError> errores) {
        //ESTA TRADUCCION TIENE QUE VER CON TODOS LOS ATRIBUTOS QUE VAN DENTRO DE LA PREGUNTA
        this.parametrosDePreguntas = new ArrayList<>();
        this.parametrosPadre = new ArrayList<>();
        this.tabs = tabs;
        String cad = "";
        try {
            //////////////////////////////////////////////////////////////////
            //INICIO DE LA PREGUNTA
            this.Etiqueta = traduceEtiqueta(ts, errores);
            if(this.Etiqueta.contains("NULL"))
            {
                this.Etiqueta ="Cadena Etiqueta = \" \";\n";
            }
            /////////////////////////////////////////////
            tabula();
            evaluaAtributos(ts, errores, tabs);
            escribeMetodoRespuesta(ts, errores);
            destabula();
            //TRADUCIRE LA ETIQUETA PRIMERO PARA SABER QUE TIENE DE PARAMETROS
            cad += dameTabulaciones()+"$$ PREGUNTA: "+Idpregunta+"\n";
            cad += dameTabulaciones()+"Pregunta "+Idpregunta+"("+cadenaParams()+"){\n";
            ///////////////////////////////////////////////////////////////////////////
            tabula();
            /////////////////////////////////////////////////////////
            cad += dameTabulaciones()+traduceTipo();//TIPO DE PREGUNTA
            cad += dameTabulaciones()+this.Etiqueta;//ETIQUETA
            for(String s : this.valorAtributosLocales)
            {
                cad += s;
            }
            /////////////////////////////////////////////////////////
            //escribeMetodoRespuesta(ts, errores);
            if(!(this.Tipo == TipoPregunta.NOTA || this.Tipo == TipoPregunta.CALCULAR || this.Tipo == TipoPregunta.FICHERO) )
            {
                //NO ESCRIBE EL METODO RESPUESTA!
                cad += this.metodoResp;
            }
            //ESCRIBE EL METODO CALCULAR SI EXSITIESE
            cad += this.metodoCalcular;
            //ESCRIBE EL METODO MOSTRAR SI EXISTE
            cad += this.metodoMostrar;
            /////////////////////////////////////////////////////////
            cad += "\n";
            destabula();
            cad += dameTabulaciones()+"}\n\n";
        } catch (Exception e) {
            //cad += "\n$$ERROR EN LA TRADUCCION DE LA PREGUNTA: "+Idpregunta;
            cad = "\n";
            errores.add(new TError("Ejecucion", "Error Ejecutando el Parser", "Pregunta: "+this.Idpregunta, "Encuesta"));
        }
        return cad;
    }

    @Override
    public Object traducirGlobal(TablaSimbolos ts, ArrayList<String> tabs, ArrayList<TError> errores) {
        String cad = "";
        if(!this.padre.equals(""))
        {
            this.padre += "().";
        }
        cad += this.padre+this.Idpregunta+"";//AQUI IRIAN LOS PARAMETROS QUE TENGO EN LA LISTA
        return cad;
    }

    @Override
    public void tabula() {
        this.tabs.add("\t");
    }

    @Override
    public void destabula() {
        if (this.tabs.size() > 0) {
            this.tabs.remove(0);
        }
    }

    @Override
    public String dameTabulaciones() {
        String cad = "";
        for (String t : this.tabs) {
            cad += t;
        }
        return cad;
    }

    String cuerpo = "";
    private void escribeMetodoRespuesta(TablaSimbolos ts, ArrayList<TError> errores)
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        cuerpo = "";
        ejecutaTraduccion("restringir", ts, errores, tabs);
        ///////////////////////////////////////////////////////////////////////////////////////////
        String tipo = this.traduceTipo();
        //tipo = tipo.replace(" Respuesta;\n", "");
        this.metodoResp = dameTabulaciones()+this.visibilidadResp+"respuesta("+this.tipoRespParam+" param_1){\n";
        
        if(this.cuerpo.equals(""))
        {
            tabula();
            this.metodoResp += dameTabulaciones()+"Respuesta = param_1;\n";
            destabula();
        }
        else
        {
            this.metodoResp += cuerpo;
        }
        this.metodoResp += this.dameTabulaciones()+"}\n";
        
    }
    
    
    String predeterminado = ";\n";
    String tipoRespParam = "";
    String tipoCalcular = "";
    boolean huboColCalculo = false;
    private String traduceTipo()
    {
        String cad = "";
        switch(this.Tipo)
        {
            case TipoPregunta.TEXTO:
            {
                //AUN SI NO HAY PREDETERMINADO
                cad += "Cadena Respuesta"+predeterminado;
                this.tipoRespParam = "Cadena";
                break;
            }
            case TipoPregunta.ENTERO:
            {
                cad += "Entero Respuesta"+predeterminado;
                this.tipoRespParam = "Entero";
                break;
            }
            case TipoPregunta.DECIMAL:
            {
                cad += "Decimal Respuesta"+predeterminado;
                this.tipoRespParam = "Decimal";
                break;
            }
            case TipoPregunta.RANGO:
            {
                cad += "Entero Respuesta"+predeterminado;
                this.tipoRespParam = "Entero";
                break;
            }
            case TipoPregunta.CONDICION:
            {
                cad += "Booleano Respuesta"+predeterminado;
                this.tipoRespParam = "Booleano";
                break;
            }
            case TipoPregunta.FECHA:
            {
                cad += "Fecha Respuesta"+predeterminado;
                this.tipoRespParam = "Fecha";
                break;
            }
            case TipoPregunta.HORA:
            {
                cad += "Hora Respuesta"+predeterminado;
                this.tipoRespParam = "Hora";
                break;
            }
            case TipoPregunta.FECHAHORA:
            {
                cad += "FechaHora Respuesta"+predeterminado;
                this.tipoRespParam = "FechaHora";
                break;
            }
            case TipoPregunta.SELEC_MULT:
            {
                cad += "Cadena Respuesta"+predeterminado;
                this.tipoRespParam = "Cadena";
                break;
            }
            case TipoPregunta.SELEC_UNO:
            {
                cad += "Cadena Respuesta"+predeterminado;
                this.tipoRespParam = "Cadena";
                break;
            }
            case TipoPregunta.NOTA:
            {
                cad += "$$ ES UNA NOTA... NO TIENE RESPUESTA\n";
                break;
            }
            case TipoPregunta.FICHERO:
            {
                cad += "$$ NO TIENE RESPUESTA: ACEPTA FICHEROS DE TIPO: "+this.ficheroExt+"\n";
                break;
            }
            case TipoPregunta.CALCULAR:
            {
                if(this.huboColCalculo)
                {
                    cad += this.tipoCalcular+predeterminado;
                }
                else
                {
                    cad += "$$ FALTA COLUMNA CALCULO! ESTO PODRIA GENERAR UN ERROR!\n";
                }
                break;
            }
        }
        return cad;
    }
    
    private  String traduceEtiqueta(TablaSimbolos ts, ArrayList<TError> errores)
    {
        int cantidad = errores.size();
        String cad = "Cadena Etiqueta = ";
        StringReader stream = new StringReader(this.Etiqueta);
        try {
            EtiqParser parse = new EtiqParser(stream);
            parse.setUp(errores, ts, padre, this.Idpregunta);
            parse.setArchivo("Encuesta", "etiqueta");
            cad += parse.INICIO();
            insertaParametros(parse.getParams());
            insertaParamasPadre(parse.getParamsPadre());
            //////////////////////
            //////////////////////
        } catch (Exception e) {
            cad += "\"Hola Mundo\"; $$ "+e.getMessage()+";\n";
        }
        return cad+=";\n";
    }
    
    
    private String cadenaParams()
    {
        String cad = "";
        for(int x = 0; x < this.parametrosDePreguntas.size(); x++)
        {
            if(!(x==this.parametrosDePreguntas.size()-1))
            {
                cad += this.parametrosDePreguntas.get(x)+", ";
            }
            else
            {
                cad += this.parametrosDePreguntas.get(x);
            }
        }
        return cad;
    }
    
    private String cadenaParamsPadre()
    {
        String cad = "";
        for(int x= 0; x< this.parametrosPadre.size(); x++)
        {
            if(!(x == this.parametrosPadre.size()-1))
            {
                cad += this.parametrosPadre.get(x)+", ";
            }
            else
            {
                cad += this.parametrosPadre.get(x);
            }
        }
        return cad;
    }
    
    
    private boolean ExistePadreID(String s)
    {
        for(String x: this.parametrosPadre)
        {
            if(x.toLowerCase().equals(s.toLowerCase()))
            {
                return true;
            }
        }
        return false;
    }
    
    private void insertaParamasPadre(ArrayList<String> params)
    {
        for(String s : params)
        {
            if(!s.equals("") && !ExistePadreID(s))
            {
                this.parametrosPadre.add(s);
            }
        }
    }
    
    private void insertaParametros(ArrayList<String> params)
    {
        for(String s : params)
        {
            if(!s.equals("") && !ExisteID(s))
            {
                this.parametrosDePreguntas.add(s);
            }
        }
    }
    
    private boolean ExisteID(String s)
    {
        for(String x : this.parametrosDePreguntas)
        {
            if(x.toLowerCase().equals(s.toLowerCase()))
            {
                return true;
            }
        }
        return false;
    }
    
    private void evaluaAtributos(TablaSimbolos ts, ArrayList<TError> errores, ArrayList<String> tabs)
    {
        //SUGERIR
        ejecutaTraduccion("sugerir", ts, errores, tabs);
        //REQUERIDO
        ejecutaTraduccion("requerido", ts, errores, tabs);
        //REQUERIDOMSN
        ejecutaTraduccion("requeridomsn", ts, errores, tabs);
        //LECTURA
        ejecutaTraduccion("lectura", ts, errores, tabs);
        //PREDETERMINADO
        ejecutaTraduccion("predeterminado", ts, errores, tabs);
        //CALCULO
        ejecutaTraduccion("calculo", ts, errores, tabs);
        //MULTIMEDIA
        ejecutaTraduccion("multimedia", ts, errores, tabs);
        
    }
    
    ArrayList<String> valorAtributosLocales;
    private void ejecutaTraduccion(String atributo, TablaSimbolos ts, ArrayList<TError> errores, ArrayList<String> tabs)
    {
        String cad = "";
        switch(atributo)
        {
            case "sugerir":
            {
                if(this.atributos.containsKey(atributo))
                {
                    Sugerencia s = (Sugerencia)this.atributos.get(atributo);
                    s.setActual(this.Idpregunta);
                    s.setPadre(padre);
                    cad += this.dameTabulaciones()+s.traducirLocal(ts, tabs, errores);
                    insertaParametros(s.getParams());
                    insertaParamasPadre(s.getParamsPadre());
                    this.valorAtributosLocales.add(cad);
                    break;
                }
                break;
            }
            case "requerido":
            {
                if(this.atributos.containsKey(atributo))
                {
                    Requerido r = (Requerido)this.atributos.get(atributo);        
                    cad += this.dameTabulaciones()+r.traducirLocal(ts, tabs, errores);
                    this.valorAtributosLocales.add(cad);
                    break;
                }
                break;
            }
            case "requeridomsn":
            {
                if(this.atributos.containsKey(atributo))
                {
                    RequeridoMsn r = (RequeridoMsn)this.atributos.get(atributo);
                    r.setActual(this.Idpregunta);
                    r.setPadre(this.padre);
                    cad += this.dameTabulaciones()+r.traducirLocal(ts, tabs, errores);
                    insertaParametros(r.getParams());
                    insertaParamasPadre(r.getParamsPadre());
                    this.valorAtributosLocales.add(cad);
                }
                break;
            }
            case "lectura":
            {
                if(this.atributos.containsKey("lectura"))
                {
                    Lectura l = (Lectura)this.atributos.get(atributo);
                    this.visibilidadResp = (String)l.traducirLocal(ts, tabs, errores);
                }
                break;
            }
            case "restringir":
            {
                if(this.atributos.containsKey("restringir"))
                {
                    Restringir r = (Restringir)this.atributos.get(atributo);
                    r.setActual("param_1");
                    r.setPadre(this.padre);
                    this.cuerpo = (String)r.traducirLocal(ts, tabs, errores);
                    insertaParametros(r.getParams());
                    insertaParamasPadre(r.getParamsPadre());
                    if(this.atributos.containsKey("restringirmsn"))
                    {
                        RestringirMsn msn = (RestringirMsn)this.atributos.get("restringirmsn");
                        msn.setActual(this.Idpregunta);
                        msn.setPadre(padre);
                        this.cuerpo += (String)msn.traducirLocal(ts, tabs, errores);
                        insertaParametros(msn.getParams());
                        insertaParamasPadre(msn.getParamsPadre());
                    }
                    //AQUI NECESITO PARAMETROS QUE SE GENERAN
                }
                break;
            }
            case "predeterminado":
            {
                if(this.atributos.containsKey("predeterminado"))
                {
                    PorDefecto p =(PorDefecto)this.atributos.get(atributo);
                    p.setActual("");
                    p.setPadre(padre);
                    this.predeterminado = (String)p.traducirLocal(ts, tabs, errores)+";\n";
                    insertaParametros(p.getParams());
                    insertaParametros(p.getParamsPadre());
                }
                break;
            }
            case "calculo":
            {
                if(this.atributos.containsKey("calculo"))
                {
                    Calculo c = (Calculo)this.atributos.get(atributo);
                    c.setActual("Respuesta");
                    c.setPadre(padre);
                    this.metodoCalcular = (String)c.traducirLocal(ts, tabs, errores);
                    insertaParametros(c.getParams());
                    insertaParamasPadre(c.getParamspPadre());
                    this.tipoCalcular = c.getTipoCalcular();
                    this.huboColCalculo = true;
                }
                break;
            }
            case "multimedia":
            {
                if(this.atributos.containsKey("multimedia"))
                {
                    Multimedia m = (Multimedia)this.atributos.get(atributo);
                    m.setActual("Respuesta");
                    m.setPadre(padre);
                    this.metodoMostrar = (String)m.traducirLocal(ts, tabs, errores);
                    insertaParametros(m.getParams());
                    insertaParametros(m.getParamsPadre());
                }
                break;
            }
        }
    }
}
