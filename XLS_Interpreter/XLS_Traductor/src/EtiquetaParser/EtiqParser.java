/* Generated By:JavaCC: Do not edit this line. EtiqParser.java */
package EtiquetaParser;
import ManejoError.TError;
import java.util.ArrayList;
import Tablas.*;
import Abstract.*;
public class EtiqParser implements EtiqParserConstants {
        ArrayList<TError> errores;
        TablaSimbolos ts;
        String columna; //LA COLUMNA QUE ESTOY TRADUCIENDO
        String archivo; //ESTE SIEMPRE SERA ENCUESTA
        String idPreguntaActual;
        String idPadre;

        ArrayList<String> params;

        ArrayList<String> paramsPadre;

        public ArrayList<String> getParamsPadre()
        {
                return this.paramsPadre;
        }
        public ArrayList<String> getParams()
        {
                return this.params;
        }
        public void setUp(ArrayList<TError> errores, TablaSimbolos ts, String padre, String actual)
        {
                setErrores(errores);
                setSimbolTable(ts);
                setPadreYActual(padre, actual);
        }

    void setErrores(ArrayList<TError> errores)
        {
                this.errores = errores;
        }

        void setSimbolTable(TablaSimbolos ts)
        {
                this.ts = ts;
        }

        public void setArchivo(String archivo, String col)
        {
                this.columna = col;
                this.archivo = archivo;
        }

        void setPadreYActual(String padre, String actual)
        {
                this.idPreguntaActual = actual;
                this.idPadre = padre;
        }

        String devuelvemeID(String id)
        {
                String aux = id;
                aux = aux.replace("#[","");
                aux = aux.replace("]","");
                return aux;
        }

        String obtTipoAltoNivel(int tipo)
        {
                switch(tipo)
                {
                        case TipoPregunta.ENTERO:
                        {
                                return "Entero";
                        }
                        case TipoPregunta.TEXTO:
                        {
                                return "Cadena";
                        }
                        case TipoPregunta.DECIMAL:
                        {
                                return "Decimal";
                        }
                        case TipoPregunta.RANGO:
                        {
                                return "Entero";
                        }
                        case TipoPregunta.CONDICION:
                        {
                                return "Booelano";
                        }
                        case TipoPregunta.FECHA:
                        {
                                return "Fecha";
                        }
                        case TipoPregunta.HORA:
                        {
                                return "Hora";
                        }
                        case TipoPregunta.FECHAHORA:
                        {
                                return "FechaHora";
                        }
                        case TipoPregunta.SELEC_MULT:
                        {
                                return "Cadena";
                        }
                        case TipoPregunta.SELEC_UNO:
                        {
                                return "Cadena";
                        }
                        case TipoPregunta.NOTA:
                        {
                                return "Cadena";
                        }
                        case TipoPregunta.FICHERO:
                        {
                                return "Cadena";
                        }
                        case TipoPregunta.CALCULAR:
                        {
                                return "Decimal";
                        }
                }
                return "";
        }

  void skip_error_recovery(int kind, String archivo, String columna) throws ParseException {
        errores.add(new TError("Sintactico","Error en la columna: "+columna,columna,archivo));
        Token t;
        do {
        t = getNextToken();
                if(t.kind == 0){break;}
    } while (t.kind != kind || t.kind != 0);
  }

//SINTAXIS
  final public String INICIO() throws ParseException {
        String cadena = "\u005c"";
        this.params = new ArrayList();
        this.paramsPadre = new ArrayList();
    cadena = S(cadena);
                {if (true) return cadena;}
    throw new Error("Missing return statement in function");
  }

  final public String S(String cad) throws ParseException {
        Token t;
        String cadena = "";
    switch ((jj_ntk==-1)?jj_ntk():jj_ntk) {
    case identificador:
      t = jj_consume_token(identificador);
                //ACCIONES
                String aux = devuelvemeID(t.image);//OBTENGO EL VALOR DEL TOKEN
                if(this.ts.existeElemento(aux.toLowerCase()))
                {
                        //SI EXISTE UN ELEMENTO CON ESE NOMBRE ENTONCES:
                        // PRIMERO VOY A TRAERLO Y VEO QUE SEA UNA INSTANCIA DE UNA PREGUNTA PARA LUEGO SABER SU TIPO Y TRADUCIRLO
                        Simbolo sim = this.ts.getSimbolo(aux);
                        String papaSim = sim.getPadre();
                        papaSim = "";//BOORE EL PARDRE!!!
                        if(!papaSim.equals("")) {papaSim += "().";}
                        if(sim.getElemento() instanceof Pregunta)
                        {
                                Pregunta p = (Pregunta)sim.getElemento();//CASTEO A PREGUNTA
                                String tipo = obtTipoAltoNivel(p.getTipo()); //OBTENGO EL TIPO
                                this.params.add(tipo+" "+aux);//ANADO A LOS PARAMETROS QUE NECESITA LA PREGUNTA
                                this.paramsPadre.add(papaSim + aux +"().Respuesta");
                        }
                        cad += "\u005c"+"+aux+"+\u005c"";
                }
                else
                {
                        //SI NO EXISTE EL ELEMENTO CON ESTE ID: ERROR SEMANTICO!
                        this.errores.add(new TError("Semantico", "Se hace referencia a una pregunta que no existe: "+aux,columna,archivo));
                        cad += "";
                }
      cadena = S(cad);
                //RETORNO
                {if (true) return cadena;}
      break;
    case cualquiera:
      t = jj_consume_token(cualquiera);
                //ACCIONES
                cad += t.image;
      cadena = S(cad);
                //RETORNO
                {if (true) return cadena;}
      break;
    default:
      jj_la1[0] = jj_gen;
      EMPTY();
                {if (true) return cad += "\u005c"";}
    }
    throw new Error("Missing return statement in function");
  }

  final public void EMPTY() throws ParseException {
                //VACIO
                System.out.println("VACIO");
  }

  /** Generated Token Manager. */
  public EtiqParserTokenManager token_source;
  SimpleCharStream jj_input_stream;
  /** Current token. */
  public Token token;
  /** Next token. */
  public Token jj_nt;
  private int jj_ntk;
  private int jj_gen;
  final private int[] jj_la1 = new int[1];
  static private int[] jj_la1_0;
  static {
      jj_la1_init_0();
   }
   private static void jj_la1_init_0() {
      jj_la1_0 = new int[] {0x6,};
   }

  /** Constructor with InputStream. */
  public EtiqParser(java.io.InputStream stream) {
     this(stream, null);
  }
  /** Constructor with InputStream and supplied encoding */
  public EtiqParser(java.io.InputStream stream, String encoding) {
    try { jj_input_stream = new SimpleCharStream(stream, encoding, 1, 1); } catch(java.io.UnsupportedEncodingException e) { throw new RuntimeException(e); }
    token_source = new EtiqParserTokenManager(jj_input_stream);
    token = new Token();
    jj_ntk = -1;
    jj_gen = 0;
    for (int i = 0; i < 1; i++) jj_la1[i] = -1;
  }

  /** Reinitialise. */
  public void ReInit(java.io.InputStream stream) {
     ReInit(stream, null);
  }
  /** Reinitialise. */
  public void ReInit(java.io.InputStream stream, String encoding) {
    try { jj_input_stream.ReInit(stream, encoding, 1, 1); } catch(java.io.UnsupportedEncodingException e) { throw new RuntimeException(e); }
    token_source.ReInit(jj_input_stream);
    token = new Token();
    jj_ntk = -1;
    jj_gen = 0;
    for (int i = 0; i < 1; i++) jj_la1[i] = -1;
  }

  /** Constructor. */
  public EtiqParser(java.io.Reader stream) {
    jj_input_stream = new SimpleCharStream(stream, 1, 1);
    token_source = new EtiqParserTokenManager(jj_input_stream);
    token = new Token();
    jj_ntk = -1;
    jj_gen = 0;
    for (int i = 0; i < 1; i++) jj_la1[i] = -1;
  }

  /** Reinitialise. */
  public void ReInit(java.io.Reader stream) {
    jj_input_stream.ReInit(stream, 1, 1);
    token_source.ReInit(jj_input_stream);
    token = new Token();
    jj_ntk = -1;
    jj_gen = 0;
    for (int i = 0; i < 1; i++) jj_la1[i] = -1;
  }

  /** Constructor with generated Token Manager. */
  public EtiqParser(EtiqParserTokenManager tm) {
    token_source = tm;
    token = new Token();
    jj_ntk = -1;
    jj_gen = 0;
    for (int i = 0; i < 1; i++) jj_la1[i] = -1;
  }

  /** Reinitialise. */
  public void ReInit(EtiqParserTokenManager tm) {
    token_source = tm;
    token = new Token();
    jj_ntk = -1;
    jj_gen = 0;
    for (int i = 0; i < 1; i++) jj_la1[i] = -1;
  }

  private Token jj_consume_token(int kind) throws ParseException {
    Token oldToken;
    if ((oldToken = token).next != null) token = token.next;
    else token = token.next = token_source.getNextToken();
    jj_ntk = -1;
    if (token.kind == kind) {
      jj_gen++;
      return token;
    }
    token = oldToken;
    jj_kind = kind;
    throw generateParseException();
  }


/** Get the next Token. */
  final public Token getNextToken() {
    if (token.next != null) token = token.next;
    else token = token.next = token_source.getNextToken();
    jj_ntk = -1;
    jj_gen++;
    return token;
  }

/** Get the specific Token. */
  final public Token getToken(int index) {
    Token t = token;
    for (int i = 0; i < index; i++) {
      if (t.next != null) t = t.next;
      else t = t.next = token_source.getNextToken();
    }
    return t;
  }

  private int jj_ntk() {
    if ((jj_nt=token.next) == null)
      return (jj_ntk = (token.next=token_source.getNextToken()).kind);
    else
      return (jj_ntk = jj_nt.kind);
  }

  private java.util.List<int[]> jj_expentries = new java.util.ArrayList<int[]>();
  private int[] jj_expentry;
  private int jj_kind = -1;

  /** Generate ParseException. */
  public ParseException generateParseException() {
    jj_expentries.clear();
    boolean[] la1tokens = new boolean[3];
    if (jj_kind >= 0) {
      la1tokens[jj_kind] = true;
      jj_kind = -1;
    }
    for (int i = 0; i < 1; i++) {
      if (jj_la1[i] == jj_gen) {
        for (int j = 0; j < 32; j++) {
          if ((jj_la1_0[i] & (1<<j)) != 0) {
            la1tokens[j] = true;
          }
        }
      }
    }
    for (int i = 0; i < 3; i++) {
      if (la1tokens[i]) {
        jj_expentry = new int[1];
        jj_expentry[0] = i;
        jj_expentries.add(jj_expentry);
      }
    }
    int[][] exptokseq = new int[jj_expentries.size()][];
    for (int i = 0; i < jj_expentries.size(); i++) {
      exptokseq[i] = jj_expentries.get(i);
    }
    return new ParseException(token, exptokseq, tokenImage);
  }

  /** Enable tracing. */
  final public void enable_tracing() {
  }

  /** Disable tracing. */
  final public void disable_tracing() {
  }

}
