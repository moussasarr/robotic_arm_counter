using System;
using System.IO.Ports;
using System.Threading;
using System.Data;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace MonoSerial
{
  class Program
  {
    public static void Main()
    {

    // hang on events 

      MySql.Data.MySqlClient.MySqlConnection conn;
      string myConnectionString;
      myConnectionString = "server=127.0.0.1;uid=root;" +
      "pwd=consulting9;database=microframe;";
      try
         {
          conn = new MySql.Data.MySqlClient.MySqlConnection();
          conn.ConnectionString = myConnectionString;
          conn.Open();
          IDbCommand dbcmd = conn.CreateCommand();

           string sql =
           "SELECT  count " +        
           "FROM microframe";
       dbcmd.CommandText = sql;
       IDataReader reader = dbcmd.ExecuteReader();   
       while(reader.Read()) {
            string count = (string) reader["count"];  
              
            Console.WriteLine("Count: "  + count); 
            }

        // clean up
         reader.Close();
         reader = null;
         dbcmd.Dispose();
         dbcmd = null;
         conn.Close();
         conn = null;
         } 

         catch (MySql.Data.MySqlClient.MySqlException ex)
        {
          Console.WriteLine(ex.Message);
        }

      int i = 0;
      bool RUNNING = true;
      while(RUNNING){
        
        Console.WriteLine("Listening for the BIG BUTTON:......... ");
        ConsoleKeyInfo name = Console.ReadKey();
        Console.WriteLine("You pressed {0}", name.KeyChar);

        Console.WriteLine(".......................................");

        
        if(name.Key == ConsoleKey.Spacebar){


          Console.WriteLine("Button was just pushed .... Opening Port....");
            SerialPort Serial_tty = new SerialPort();
            string ttyname;
            ttyname = "/dev/tty.usbserial";


            Serial_tty.PortName = ttyname;            // assign the port name 
            Serial_tty.BaudRate = 9600;               // Baudrate = 9600bps
            Serial_tty.Parity   = Parity.None;        // Parity bits = none  
            Serial_tty.DataBits = 8;                  // No of Data bits = 8
            Serial_tty.StopBits = StopBits.One;       // No of Stop bits = 1
            Serial_tty.Handshake = Handshake.None;
            
     try
     {
        Serial_tty.Open();                 // Open the port   
        i++; 
        string output = i.ToString()+"\r";                 
        Serial_tty.Write(output);           // Write an ascii "1"
        int milliseconds = 250;
        Thread.Sleep(milliseconds);            //Wait for 250 milliseconds
        Serial_tty.Close();               // Close port
        Console.WriteLine("\nA written to port {0}", Serial_tty.PortName);

     }
     catch(Exception e)
     {
       Console.WriteLine("Error in Opening {0}",Serial_tty.PortName);
       Console.WriteLine(e);
     }

        }

      }
      
    }
  }
}