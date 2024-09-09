//using ChatGS.Models;
//using System;
//using System.Threading.Tasks;

//namespace ChatGS.Services
//{
//    public class KafkaProducerService 
//    {
//        private readonly IProducer<Null, string> _producer;
//        private readonly string _topico;

//        public KafkaProducerService(string bootstrapServers, string topico)
//        {
//            var config = new ProducerConfig { BootstrapServers = bootstrapServers };
//            _producer = new ProducerBuilder<Null, string>(config).Build();
//            _topico = topico;
//        }

//        public async Task EnviarMensagemAsync(MensagensModel mensagem)
//        {
//            try
//            {
//                var dr = await _producer.ProduceAsync(_topico, new Message<Null, string> { Value = mensagem.Conteudo });
//                Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
//            }
//            catch (ProduceException<Null, string> e)
//            {
//                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
//            }
//        }
//    }
//}
