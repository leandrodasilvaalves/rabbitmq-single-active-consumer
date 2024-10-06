# SAC - Single Active Consumer
- O objetivo desse repositório é validar o comportamento do SAC, feature existente desde o RabbitMQ 3.8.

> Single Active Consumer (SAC) is a really nice feature that is coming with release 3.8 this year. It allows for new consumer patterns that were not possible or more difficult to achieve beforehand.

## Métrica
Usar a métrica abaixo no prometheus para avaliar a diferença de rate entre 3 consumers e um único consumidor processando as mensagens.
```
sum(rate(consumer_processing_time_count[1m]))
```
Fiz testes utilizando uma carga de 500 mensagens para cada cenário.
1. Com três consumidores, retornou um rate de 1.5. Ou seja, estava processando uma mensagem e meia a cada segundo.
2. Com um único consumidor (SAC), retornou um rate de 0.5. Ou seja, está levando até 2 segundos para processar uma mensagem

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