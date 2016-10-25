package application;

import java.math.BigDecimal;

public class DotPressedCalculatorState extends BaseCalculatorState {

	protected DotPressedCalculatorState(BigDecimal accumulator, BigDecimal screenValue,
			CalculatorOperator currentCalculatorOperator) {
		super(accumulator, screenValue, currentCalculatorOperator);
	}

	@Override
	public Calculator numericButtonPressed(int button) {
		if( thereIsPlaceForMoreDigitsOnScreen()){
			int numberOfDecimalPlaces = getNumberOfDecimalPlaces(screenValue);
			BigDecimal thingToAdd = new BigDecimal("0.1").pow(numberOfDecimalPlaces + 1)
					.multiply(new BigDecimal(button));
			screenValue = screenValue.add(thingToAdd);
			return new ScreenViewCalculatorState(accumulator, screenValue, currentCalculatorOperator);
		} else {
			return new ScreenViewCalculatorState(accumulator, screenValue, currentCalculatorOperator);
		}
	}

	@Override
	public Calculator operatorButtonPressed(CalculatorOperator operator) {
		return new ScreenViewCalculatorState(accumulator, screenValue, currentCalculatorOperator).operatorButtonPressed(operator);
	}

	@Override
	public Calculator dotButtonPressed() {
		return new DotPressedCalculatorState(accumulator, screenValue, currentCalculatorOperator);
	}

	@Override
	public Calculator clearButtonPressed() {
		return new ScreenViewCalculatorState(accumulator,screenValue,currentCalculatorOperator).clearButtonPressed();
	}

	@Override
	public Calculator invertSignButtonPressed() {
		return new ScreenViewCalculatorState(accumulator, screenValue, currentCalculatorOperator).invertSignButtonPressed();
	}

	@Override
	public Calculator squareRootButtonPressed() {
		return new ScreenViewCalculatorState(accumulator, screenValue, currentCalculatorOperator).squareRootButtonPressed();
	}

	@Override
	public Calculator equalButtonPressed() {
		return new ScreenViewCalculatorState(accumulator, screenValue, currentCalculatorOperator).equalButtonPressed();
	}

	@Override
	public String getDisplayedValue() {
		if( screenValue.scale() == 0 ){
			if( thereIsPlaceForMoreDigitsOnScreen()){
				return bigDecimalToShortString(screenValue)+".";
			} else {
				return bigDecimalToShortString(screenValue);
			}
		} else {
			return  bigDecimalToShortString(screenValue);
		}
	}

}
