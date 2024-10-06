# SAC - Single Active Consumer
- O objetivo desse repositório é validar o comportamento do SAC, feature existente desde o RabbitMQ 3.8.

> Single Active Consumer (SAC) is a really nice feature that is coming with release 3.8 this year. It allows for new consumer patterns that were not possible or more difficult to achieve beforehand.

## Conclusão
- Este é um recurso nativo do RabbitMQ a partir da versão 3.8 e não do Masstransit
- Por mais que hajam múltiplas instâncias de consumers plugados na fila, apenas 1 irá consumir as mensagens
- Se o consumer ativo sofrer shutdown, um dos outros consumers irão assumir o posto de SAC
- **[ATENÇÃO]** Ativar o recurso para uma fila existente não irá funcionar. 
    * A fila precisa ser recriada, do contrário os consumers não serão capazes de plugar na fila
    * As conexões aparecem ativas, mas nenhum container é plugado na fila
    * Encontrei dois cenários para resolver:
        1. Recriar a fila se for possível
        2. Se não puder recriar a fila. Criar uma fila nova e mover as mensagens da fila antiga para a nova


# Referência
- https://www.cloudamqp.com/blog/rabbitmq-3-8-feature-focus-single-active-consumer.html