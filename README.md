# TaaS - Traffic as a Service
Mit Hilfe dieses Services, zugegebenermaßen nicht ernstgemeint und eher ein ProofOfConcept, ist es möglich per Knopfdruck 100MB an Daten in Sekunden von seinem Datenvolumen zu verbrauchen.

## Hintergrund
Der Hintergrund dieses Services ist die Möglichkeit, dass die Telekom Deutschland (andere ISPs bisher nicht getestet) über IPv6 eingehenden UDP und TCP Traffic bis an das Endgerät des Kunden erlaubt.
Bisher scheint ausschließlich eingehender, nicht related, ICMPv6 Traffic blockiert zu werden, um nicht das Datenvolumen des Kunden mittels ICMPv6 Echo Requests zu verbrauchen. 

Wir haben das ganze nun auf die Spitze getrieben und uns zu Nutze gemacht, dass UDP Verbindungslos ist und eingehender UDP/IPv6 Traffic nicht direkt im Telekom Backbone blockiert wird.
Folglich kann man unproblematisch UDP Pakete an die IPv6-Adresse, die sich mit dem Webserver verbindet, schicken, wobei es unerheblich ist, ob auf dem Endgerät auch wirklich ein UDP Server auf Port 4242 lauscht.

Die Datenabrechnung erfolgt allerdings scheinbar nicht auf Basis wie viele Daten wirklich beim Endgerät angekommen sind, sondern wie viele Daten, die für den Kunden bestimmt sind, durch das Telekom Backbone, geschickt wurden.

Verbindet man sein Endgerät nun über 2G mit dem Telekom Mobilfunk, so können innerhalb von 5s unmöglich 100MB an das Endgerät geschickt werden, sodass der Traffic vermutlich bei der Übertragung zwischen Backbone und Endgerät gedroppt wird.
Dennoch werden 100MB beim Kunden abgerechnet.

Der ganze Spaß entstand auf Basis dieses Tweets:
https://twitter.com/tschaeferm/status/1223319089730539520

## Problem
Es reicht ausschließlich die IPv6-Adresse eine beliebigen Endgerätes im Mobilfunknetz zu kennen. Somit könnte ein Angreifer rein durch das Aufrufen einer Webseite das gesamte Datenvolumen eines Kunden verbrauchen ohne z.B. explizit eine Datei herunterladen zu müssen.

Der Verbrauch des Datenvolumens ist außerdem nicht auf 100MB beschränkt. So lässt sich mit einfachen Linux Boardmitteln auch unproblematisch komplett `/dev/urandom` an das Mobilfunkendgerät schicken, wie wir hier ausprobiert haben:
https://twitter.com/maichelmann/status/1226242471291768832

Ich trauere immer noch über mein Datenvolumen :).

## Lösung
Es wäre sinnvoll eingehenden UDP/IPv6 Traffic bereits im Telekom Backbone zu blockieren und somit das Datenvolumen der Kunden nicht zu belasten.

Dabei wäre es aber durchaus wünschenswert weiterhin TCP/IPv6 von außen zu erlauben, da dies neue Möglichkeiten zur Nutzung des Mobilfunknetztes eröffnet.

# DISCLAIMER
Dieser "Service" ist aus Spaß und Nerdigkeit entstanden und verfolgt keine anderen Absichten. Wir übernehmen außerdem keine Haftung und Kosten für das verbrauchte Datenvolumen.
