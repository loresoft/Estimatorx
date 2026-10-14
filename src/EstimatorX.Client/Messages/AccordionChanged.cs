namespace EstimatorX.Client.Messages;

public record AccordionChanged(
    string ParentGroup,
    long AccordionId,
    bool Collapsed
);
